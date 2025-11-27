using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MoveBlockScript : MonoBehaviour
{
    [SerializeField] private Material selectedMaterial, defaultMaterial;
    [SerializeField] private UIToggles uiToggle;
    [SerializeField] private PlayerMouseMovement playerMovement;
    private BlockPuzzleManager manager;
    [SerializeField] private bool objectAbleToMove;
    public bool isRightDirection; //set for every object
    public float wallDistance, playerDistance;
    public int layer;
    private int layerAsLayerMask;
    private MeshRenderer meshRenderer;

    [SerializeField] AnimationCurve stepEase = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
    [SerializeField] AnimationCurve stepHeightShape = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepDuration = 0.3f;
    private float stepTime;

    private Vector3 objectCurrentPos, objectTargetPos, playerTargetPos, playerCurrentPos;
    [SerializeField] private bool isMoving;
    [SerializeField] private bool playerIsFront; //on which side the player is (true = front, false = back)

    private void Awake() //set values
    {
        uiToggle = FindFirstObjectByType(typeof(UIToggles)) as UIToggles;
        playerMovement = FindFirstObjectByType(typeof(PlayerMouseMovement)) as PlayerMouseMovement;
        manager = FindFirstObjectByType<BlockPuzzleManager>();
        isMoving = false;
        layerAsLayerMask = (1 << layer);
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (isMoving) 
        {
            Move();
        }
    }

    public void EnteredTriggerInChild(Collider c, bool isFront)
    {
        //Debug.Log(c.name.ToString() + " detected");
        uiToggle.EnteredTrigger(this);
        playerMovement.canBeOverUI = true;
        playerIsFront = isFront;
    }

    public void ExitedTriggerInChild(Collider c)
    {
        //Debug.Log(c.name.ToString() + " left");
        uiToggle.ExitedTrigger(this);
        playerMovement.canBeOverUI = false;
    }

    public void HoldBlock()
    {
        playerMovement.allowedToMove = false;
        objectAbleToMove = true;
        meshRenderer.material = selectedMaterial;
    }
    public void LetGoOfBlock()
    {
        objectAbleToMove = false;
        playerMovement.allowedToMove = true;
        meshRenderer.material = defaultMaterial;
    }

    public void CheckIfAllowedToMove()
    {
        Vector3 rayDirect = new Vector3();
        int checkTimes = 2;
        bool[] allowedMovement = new bool[checkTimes];
        RaycastHit hit;
        //when close to walls
        for (int i = 0; i <= checkTimes -1; i++)
        {
            switch(i)
            {
                case 0:     rayDirect = transform.forward;
                    break;
                case 1:     rayDirect = -transform.forward;
                    break;
            }
            if (Physics.Raycast(transform.position, rayDirect, out hit, 3f, layerAsLayerMask))
            {
                Debug.DrawRay(transform.position, rayDirect * hit.distance, Color.green, 2f);
                float allowedDistance;
                if ((playerIsFront && rayDirect == transform.forward) || (!playerIsFront && rayDirect == -transform.forward))
                {// player is in front and moving that direction
                    allowedDistance = playerDistance;
                }
                else { allowedDistance = wallDistance; }

                if (hit.distance <= allowedDistance)
                {//too close
                    allowedMovement[i] = false;
                }
                else
                {// able to move
                    allowedMovement[i] = true;
                }
            }
            else
            {//far from anything
                Debug.DrawRay(transform.position, rayDirect * 3f, Color.red, 2f);
                allowedMovement[i] = true;
            }
        }
        // set arrows
        uiToggle.ActivateDirection(allowedMovement);
    }

    public void MoveBlock(string direction) //move 1 space
    {
        //Debug.Log("pushed " + direction.ToString());
        objectCurrentPos = this.gameObject.transform.position;
        playerCurrentPos = playerMovement.transform.position;
        if (objectAbleToMove && !isMoving)
        {
            stepTime = 0f;
            switch (direction)
            {
                case "RightUp":
                    objectTargetPos = objectCurrentPos + new Vector3(1f, 0f, 0f);
                    playerTargetPos = playerCurrentPos + new Vector3(1f, 0f, 0f);
                    break;
                case "LeftUp":
                    objectTargetPos = objectCurrentPos + new Vector3(0f, 0f, 1f);
                    playerTargetPos = playerCurrentPos + new Vector3(0f, 0f, 1f);
                    break;
                case "RightDown":
                    objectTargetPos = objectCurrentPos + new Vector3(0f, 0f, -1f);
                    playerTargetPos = playerCurrentPos + new Vector3(0f, 0f, -1f);
                    break;
                case "LeftDown":
                    objectTargetPos = objectCurrentPos + new Vector3(-1f, 0f, 0f);
                    playerTargetPos = playerCurrentPos + new Vector3(-1f, 0f, 0f);
                    break;
            }
            isMoving = true;
            //playerMovement.MoveOutsideScript(playerTargetPos);
        }
    }

    private void Move() //smooth move block
    {
        stepTime += Time.deltaTime;
        float progress = stepEase.Evaluate(stepTime / stepDuration);

        Vector3 newPos = Vector3.Lerp(objectCurrentPos, objectTargetPos, progress);
        newPos.y += stepHeight * stepHeightShape.Evaluate(progress);

        //Debug.Log("targetPos: " + newPos.ToString());
        playerMovement.MoveOutsideScript(playerTargetPos);
        this.gameObject.transform.position = newPos;
        if (progress >= 1f)
        {
            isMoving = false;
            CheckIfAllowedToMove();
            manager.checkDoubleTriggers();
        }
    }
}

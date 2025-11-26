using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MoveBlockScript : MonoBehaviour
{
    [SerializeField] private UIToggles uiToggle;
    [SerializeField] private PlayerMouseMovement playerMovement;
    [SerializeField] private bool objectAbleToMove;
    public bool isRightDirection; //set for every object
    public int layer;
    private int layerAsLayerMask;

    [SerializeField] AnimationCurve stepEase = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
    [SerializeField] AnimationCurve stepHeightShape = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepDuration = 0.3f;
    private float stepTime;

    private Vector3 objectCurrentPos, objectTargetPos, playerTargetPos, playerCurrentPos;
    [SerializeField] private bool isMoving;
    [SerializeField] private bool playerIsFront; //on which side the player is (true = front, false = back)

    private void Awake()
    {
        uiToggle = FindFirstObjectByType(typeof(UIToggles)) as UIToggles;
        playerMovement = FindFirstObjectByType(typeof(PlayerMouseMovement)) as PlayerMouseMovement;
        isMoving = false;
        layerAsLayerMask = (1 << layer);
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
        uiToggle.ExitedTrigger();
        playerMovement.canBeOverUI = false;
    }

    public void HoldBlock()
    {
        playerMovement.allowedToMove = false;
        objectAbleToMove = true;
    }
    public void LetGoOfBlock()
    {
        objectAbleToMove = false;
        playerMovement.allowedToMove = true;
    }

    public void CheckIfAllowedToMove(string direction)
    {
        Vector3 rayDirect = new Vector3();
        switch (direction)
        {
            case "RightUp":
                rayDirect = transform.forward;
                break;
            case "LeftUp":
                rayDirect = transform.forward;
                break;
            case "RightDown":
                rayDirect = -transform.forward;
                break;
            case "LeftDown":
                rayDirect = -transform.forward;
                break;
        }

        RaycastHit hit;
        if(Physics.Raycast(transform.position, rayDirect, out hit, 3f, layerAsLayerMask))
        {
            //Debug.DrawRay(transform.position, rayDirect * hit.distance, Color.green, 2f);
            //Debug.Log("object hit: " + hit.transform.name.ToString());
            
            int allowedDistance;
            if ((playerIsFront && rayDirect == transform.forward) || (!playerIsFront && rayDirect == -transform.forward))
            {// player is in front and moving that direction
                allowedDistance = 2;
            }
            else { allowedDistance = 1; }
            //Debug.Log("allowedDistance: " + allowedDistance.ToString());

            if (hit.distance <= allowedDistance)
            {//too close
                Debug.Log("too close to move");
            }
            else
            {// able to move
                MoveBlock(direction);
            }
        }
        else
        {
            //Debug.DrawRay(transform.position, rayDirect * 3f, Color.red, 2f);
            //Debug.Log("missed");
            MoveBlock(direction);
        }
        
        //if yes
        //MoveBlock(direction);
    }

    private void MoveBlock(string direction)
    {//move 1 space
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

    private void Move()
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
        }
    }
}

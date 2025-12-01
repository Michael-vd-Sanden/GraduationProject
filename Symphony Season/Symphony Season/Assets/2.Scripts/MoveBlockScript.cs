using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MoveBlockScript : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    [SerializeField] private PlayerMouseMovement playerMovement;
    private BlockPuzzleManager manager;
    public bool objectAbleToMove;
    public bool isRightDirection; //set for every object
    public bool upAllowed;
    public bool downAllowed;

    //smooth movement
    [SerializeField] AnimationCurve stepEase = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
    [SerializeField] AnimationCurve stepHeightShape = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepDuration = 0.3f;
    private float stepTime;

    private Vector3 objectCurrentPos, objectTargetPos, playerTargetPos, playerCurrentPos;
    [SerializeField] private bool isMoving;
    public bool playerIsFront; //on which side the player is (true = front, false = back)
    public float playerDistance;
    public float wallDistance;

    private void Awake()
    {
        manager = FindFirstObjectByType<BlockPuzzleManager>();
        playerMovement = FindFirstObjectByType<PlayerMouseMovement>();
        meshRenderer = GetComponent<MeshRenderer>();
        isMoving = false;
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
        playerIsFront = isFront;
        manager.EnteredTrigger(this);
    }

    public void ExitedTriggerInChild(Collider c)
    {
        //Debug.Log(c.name.ToString() + " left");
        manager.ExitedTrigger(this);
    }

    public void MoveBlock(string direction)
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
            //playerMovement.agent.isStopped = true;
            manager.CheckIfAllowedToMove();
        }
    }
}

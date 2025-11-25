using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MoveBlockScript : MonoBehaviour
{
    [SerializeField] private UIToggles uiToggle;
    [SerializeField] private PlayerMouseMovement playerMovement;
    [SerializeField] private bool objectAbleToMove;
    public bool isRightDirection; //set for every object

    [SerializeField] AnimationCurve stepEase = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
    [SerializeField] AnimationCurve stepHeightShape = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepDuration = 0.3f;
    private float stepTime;

    private Vector3 objectCurrentPos, objectTargetPos, playerTargetPos, playerCurrentPos;
    [SerializeField] private bool isMoving;

    private void Awake()
    {
        uiToggle = FindFirstObjectByType(typeof(UIToggles)) as UIToggles;
        playerMovement = FindFirstObjectByType(typeof(PlayerMouseMovement)) as PlayerMouseMovement;
        isMoving = false;
    }

    private void Update()
    {
        if (isMoving) 
        {
            Move();
        }
    }

    public void EnteredTriggerInChild(Collider c)
    {
        //Debug.Log(c.name.ToString() + " detected");
        uiToggle.EnteredTrigger(this);
        playerMovement.canBeOverUI = true;
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

    public void MoveBlock(string direction)
    {//move 1 space
        Debug.Log("pushed " + direction.ToString());
        objectCurrentPos = this.gameObject.transform.position;
        playerCurrentPos = playerMovement.transform.position;
        if (objectAbleToMove && !isMoving)
        {
            stepTime = 0f;
            isMoving = true;
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

            playerMovement.MoveOutsideScript(playerTargetPos);
            
        }
    }

    private void Move()
    {
        stepTime += Time.deltaTime;
        float progress = stepEase.Evaluate(stepTime / stepDuration);

        Vector3 newPos = Vector3.Lerp(objectCurrentPos, objectTargetPos, progress);
        newPos.y += stepHeight * stepHeightShape.Evaluate(progress);

        Debug.Log("targetPos: " + newPos.ToString());

        this.gameObject.transform.position = newPos;
        if (progress >= 1f)
        {
            isMoving = false;
        }
    }
}

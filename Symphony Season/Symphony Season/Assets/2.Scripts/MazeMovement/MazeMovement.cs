using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MazeMovement : MonoBehaviour
{
    [Header("-------------- Required Objects")]
    public PlayerMouseMovement playerMovement;
    [SerializeField] private MazePuzzle mazePuzzle;
    [SerializeField] private GameObject mazeObject;
    [SerializeField] private NavMeshSurface navMash;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PlayerFollower playerFollow;
    [SerializeField] private GameObject left, right, up, down;
    private Transform globalRoot;

    [Header("-------------- Changeble Values")]
    [SerializeField] private float turnSpeed = 1.0f;
    [SerializeField] private int layer;
    [SerializeField] private float hitDistance = 1.0f;

    [Header("-------------- Background Values (do not change)")]
    [SerializeField] List<Quaternion> availableAngles;
    [SerializeField] private int currentAngleID;
    [SerializeField] private bool mazeIsMoving, playerIsMoving;
    private float offsetAngle, startAngle;
    private Quaternion currentAngle, targetAngle;
    private int layerAsLayerMask, directionAmountActive;
    private string direction;
    private Vector3 playerCurrentPos, playerTargetPos;

    private void Awake()
    {
        playerMovement.isInMaze = true;
        currentAngleID = 0;
        globalRoot = GameObject.FindGameObjectWithTag("GlobalRoot").transform;
        layerAsLayerMask = (1 << layer);
    }
    private void Start()
    {
        CheckPlayerDirections();
    }

    private void Update()
    {
        if(mazeIsMoving) 
        {
            playerFollow.ToggleMoving(1);
            currentAngle = mazeObject.transform.rotation;

            var step = turnSpeed * Time.deltaTime;
            mazeObject.transform.rotation = Quaternion.RotateTowards(currentAngle, targetAngle, step);


           if (currentAngle == targetAngle)
           { 
                playerFollow.ToggleMoving(0);
                navMash.BuildNavMesh();
                playerMovement.allowedToMove = true;
                CheckPlayerDirections();
                mazeIsMoving = false;
            }
        }
        if(playerIsMoving) 
        {
            if(!playerMovement.isMoving)
            {// stopped moving
                CheckPlayerDirections();
                playerIsMoving = false;
            }
        }
    }

    public void MovePlayer(string inputDirection)
    {
        if (playerMovement.allowedToMove && !mazeIsMoving)
        {
            playerCurrentPos = playerMovement.transform.position;
            switch (inputDirection)
            {
                case "Up":
                    playerTargetPos = playerCurrentPos + new Vector3(2f, 0f, 0f);
                    playerMovement.MoveOutsideScript(playerTargetPos);
                    playerIsMoving = true;
                    break;
                case "Right":
                    direction = inputDirection;
                    playerTargetPos = playerCurrentPos + new Vector3(0f, 0f, -2f);
                    CheckIfCanRotate();
                    break;
                case "Down":
                    playerTargetPos = playerCurrentPos + new Vector3(-2f, 0f, 0f);
                    playerMovement.MoveOutsideScript(playerTargetPos);
                    playerIsMoving = true;
                    break;
                case "Left":
                    direction = inputDirection;
                    playerTargetPos = playerCurrentPos + new Vector3(0f, 0f, 2f);
                    CheckIfCanRotate();
                    break;

            }
        }
    }

    private void CheckPlayerDirections()
    {
        Transform t = globalRoot;
        Vector3 playerPos = playerMovement.transform.position + new Vector3(0f, -2.5f, 0f);
        Vector3 rayDirect;
        bool able;
        directionAmountActive = 0;

        for (int check = 0; check < 4; check++)
        {
            switch (check)
                {
                    case 0:
                        rayDirect = t.forward;  //left
                        break;
                    case 1:
                        rayDirect = t.right;    //up
                        break;
                    case 2:
                        rayDirect = -t.right;   //down
                        break;
                    case 3:
                        rayDirect = -t.forward; //right
                        break;
                    default:
                        rayDirect = t.forward;
                        break;
                }

            RaycastHit hit;
            if (Physics.Raycast(playerPos, rayDirect, out hit, hitDistance, layerAsLayerMask))
                {

                    if (hit.distance <= hitDistance)
                    {//too close
                        //Debug.Log("hit " + hit.collider.name);
                        //Debug.DrawRay(playerPos, rayDirect * hitDistance, Color.red, 4f);
                        able = true;
                    }
                    else
                    {
                        //Debug.DrawRay(playerPos, rayDirect * hitDistance, Color.green, 4f);
                        able = false;
                    }
                }
            else
                {
                    //Debug.DrawRay(playerPos, rayDirect * hitDistance, Color.green, 4f);
                    able = false;
                }

            switch (check)
            {
                case 0:
                    ActivatePlayerDirections("Left", able);
                    break;
                case 1:
                    ActivatePlayerDirections("Up", able);
                    break;
                case 2:
                    ActivatePlayerDirections("Down", able);
                    break;
                case 3:
                    ActivatePlayerDirections("Right", able);
                    break;
            }
        }
    }

    private void ActivatePlayerDirections(string direction, bool active)
    {
        switch (direction)
        {
            case "Left":
                if (active) { left.SetActive(true); directionAmountActive++; }
                else { left.SetActive(false); }
                break;
            case "Right":
                if (active) { right.SetActive(true); directionAmountActive++;  }
                else { right.SetActive(false); }
                break;
            case "Up":
                if (active) { up.SetActive(true); directionAmountActive++;  }
                else { up.SetActive(false); }
                break;
            case "Down":
                if (active) { down.SetActive(true); directionAmountActive++;  }
                else { down.SetActive(false); }
                break;
        }
        if(directionAmountActive > 2) { mazePuzzle.StartQuestion(); }
    }

    private void CheckIfCanRotate()
    {
        var path = new NavMeshPath();
        agent.CalculatePath(playerTargetPos, path);
        switch (path.status)
        {
            case NavMeshPathStatus.PathComplete:
                RotateMaze();
                break;
            default:
                Debug.Log("Can't move there");
                break;
        }
    }

    private void RotateMaze()
    {
        if (!mazeIsMoving && playerMovement.allowedToMove)
        {
            switch (direction)
            {
                case "Left":
                    if (currentAngleID != 19)
                    { currentAngleID++; }
                    else { currentAngleID = 0; }
                    break;
                case "Right":
                    if (currentAngleID != 0)
                    { currentAngleID--; }
                    else { currentAngleID = 19; }
                    break;
            }
            playerMovement.allowedToMove = false;
            mazeIsMoving = true;
            direction = string.Empty;
            startAngle = mazeObject.transform.rotation.eulerAngles.x;
            targetAngle = availableAngles[currentAngleID];
        }
    }
}

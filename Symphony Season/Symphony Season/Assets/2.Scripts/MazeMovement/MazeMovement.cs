using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MazeMovement : MonoBehaviour
{
    public PlayerMouseMovement playerMovement;
    [SerializeField] private GameObject mazeObject;
    [SerializeField] private NavMeshSurface navMash;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PlayerFollower playerFollow;

    private string direction;
    [SerializeField] private bool mazeIsMoving;
    private float offsetAngle, startAngle;
    private Quaternion currentAngle, targetAngle;
    [SerializeField] private float turnSpeed = 1.0f;

    [SerializeField] List<Quaternion> availableAngles;
    [SerializeField] private int currentAngleID;

    private Vector3 playerCurrentPos, playerTargetPos;

    private void Awake()
    {
        playerMovement.isInMaze = true;
        currentAngleID = 0;
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
                mazeIsMoving= false;
                playerFollow.ToggleMoving(0);
                navMash.BuildNavMesh();
                playerMovement.allowedToMove = true;
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
                    break;
                case "Right":
                    direction = inputDirection;
                    playerTargetPos = playerCurrentPos + new Vector3(0f, 0f, -2f);
                    CheckIfCanRotate();
                    break;
                case "Down":
                    playerTargetPos = playerCurrentPos + new Vector3(-2f, 0f, 0f);
                    playerMovement.MoveOutsideScript(playerTargetPos);
                    break;
                case "Left":
                    direction = inputDirection;
                    playerTargetPos = playerCurrentPos + new Vector3(0f, 0f, 2f);
                    CheckIfCanRotate();
                    break;

            }
        }
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

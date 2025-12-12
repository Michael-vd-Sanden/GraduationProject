using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class MazeMovement : MonoBehaviour
{
    public PlayerMouseMovement playerMovement;
    [SerializeField] private GameObject mazeObject;
    [SerializeField] private NavMeshSurface navMash;

    private string direction;
    [SerializeField] private bool isMoving;
    private float offsetAngle, startAngle;
    private Quaternion currentAngle, targetAngle;
    [SerializeField] private float turnSpeed = 1.0f;

    [SerializeField] List<Quaternion> availableAngles;
    [SerializeField] private int currentAngleID;

    private void Awake()
    {
        playerMovement.isInMaze = true;
        currentAngleID = 0;
    }

    private void Update()
    {
        if(playerMovement.mazeMovedLeft) { direction = "Left"; RotateMaze(); }
        if(playerMovement.mazeMovedRight) { direction = "Right"; RotateMaze(); }

        if(isMoving) 
        {
            currentAngle = mazeObject.transform.rotation;

            var step = turnSpeed * Time.deltaTime;
            mazeObject.transform.rotation = Quaternion.RotateTowards(currentAngle, targetAngle, step);


           if (currentAngle == targetAngle)
           { 
                isMoving= false;
                playerMovement.allowedToMove = true;
                navMash.BuildNavMesh();
            }
        }
    }

    private void RotateMaze()
    {
        if (!isMoving)
        {
            switch (direction)
            {
                case "Right":
                    if (currentAngleID != 19)
                    { currentAngleID++; }
                    else { currentAngleID = 0; }
                    break;
                case "Left":
                    if (currentAngleID != 0)
                    { currentAngleID--; }
                    else { currentAngleID = 19; }
                    break;
            }
            isMoving = true;
            direction = string.Empty;
            playerMovement.mazeMovedLeft = false;
            playerMovement.mazeMovedRight = false;
            playerMovement.allowedToMove = false;

            startAngle = mazeObject.transform.rotation.eulerAngles.x;
            targetAngle = availableAngles[currentAngleID];
        }
    }
}

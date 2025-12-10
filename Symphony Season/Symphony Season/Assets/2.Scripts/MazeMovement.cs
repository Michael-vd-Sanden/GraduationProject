using System.Collections;
using UnityEngine;

public class MazeMovement : MonoBehaviour
{
    public PlayerMouseMovement playerMovement;
    [SerializeField] private GameObject mazeObject;

    private string direction;
    [SerializeField] private bool isMoving;
    private float targetAngle, offsetAngle, startAngle, currentAngle;

    float turnSpeed = 0.01f;
    Quaternion rotationTarget;
    Vector3 rotationDirection;

    private void Awake()
    {
        playerMovement.isInMaze = true;
    }

    private void Update()
    {
        if(playerMovement.mazeMovedLeft) { direction = "Left"; RotateMaze(); }
        if(playerMovement.mazeMovedRight) { direction = "Right"; RotateMaze(); }

        if(isMoving) 
        {
            currentAngle = mazeObject.transform.localRotation.eulerAngles.x;

            rotationDirection = (new Vector3(0, targetAngle, 0) - new Vector3(0, currentAngle, 0)).normalized;
            rotationTarget = Quaternion.LookRotation(rotationDirection);
            mazeObject.transform.rotation = Quaternion.Slerp(mazeObject.transform.rotation, rotationTarget, turnSpeed);

            //mazeObject.transform.Rotate((Mathf.SmoothStep(startAngle, targetAngle, t)),0,0);
            //isMoving = false;
            //mazeObject.transform.rotation = Quaternion.Euler(angle,0,0);



            if (currentAngle == targetAngle)
            { isMoving= false;
              }
        }
    }

    private IEnumerator Rotate() //hier iets mee proberen, elke wait for seconds de waarde iets dichter bij het ding (of een for loop in een gewone method)
    {
        yield return new WaitForSecondsRealtime(0.1f);
    }

    private void RotateMaze()
    {
        switch (direction) 
        {
            case "Left":
                offsetAngle = -18f;
                isMoving = true;
                break;
            case "Right":
                offsetAngle = 18f;
                isMoving = true;
                break;
        }
        direction= string.Empty;
        playerMovement.mazeMovedLeft = false;
        playerMovement.mazeMovedRight = false;

        startAngle = mazeObject.transform.localRotation.eulerAngles.x;
        targetAngle = startAngle + offsetAngle;
        Debug.Log("Target: " + targetAngle);
        Debug.Log("Offset: " + offsetAngle);
        Debug.Log("Start: " + startAngle);
    }
}

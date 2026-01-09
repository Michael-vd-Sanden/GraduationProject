using UnityEngine;

public class TurnObject : MonoBehaviour
{
    //[Header("-------------- Required Objects")]
    [Header("-------------- Changeble Values")]
    [SerializeField] private float turnSpeedSeconds = 1.0f;
    [SerializeField] private float turnSpeedModifier = 180f, turnSpeed;
    [SerializeField] private Quaternion toAngle, halfwayToAngle, halfwayFromAngle;

    [Header("-------------- Background Values (do not change)")]
    public bool isRotating = false;
    [SerializeField] private Quaternion currentAngle, startAngle, targetAngle;

    private void Awake()
    {
        startAngle = transform.rotation;
        turnSpeed = turnSpeedSeconds * turnSpeedModifier;
    }

    private void Update()
    {
        if(isRotating) 
        { 
            currentAngle = transform.rotation;

            var step = turnSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(currentAngle, targetAngle, step);

            if(currentAngle == halfwayToAngle) { targetAngle = toAngle; }
            if(currentAngle == halfwayFromAngle) { targetAngle = startAngle; }
            if((targetAngle == startAngle || targetAngle == toAngle) && currentAngle == targetAngle) 
            { 
                isRotating= false;
            }
        }
    }

    public void StartRotation()
    {
        if (!isRotating)
        {
            currentAngle = transform.rotation;
            if (currentAngle == startAngle)
            { 
                targetAngle = halfwayToAngle; 
            }
            else
            { 
                targetAngle = halfwayFromAngle; 
            }

            isRotating = true;
        }
    }

    public void SetCurrent()
    {
        currentAngle = transform.rotation;
    }
}

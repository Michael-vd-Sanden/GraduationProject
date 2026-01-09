using UnityEngine;

public class MoveObject : MonoBehaviour
{
    [Header("-------------- Changeble Values")]
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private Vector3[] positions;

    [Header("-------------- Background Values (do not change)")]
    [SerializeField] private bool isMoving = false;
    [SerializeField] private int pos;
    [SerializeField] private Vector3 currentPos, targetPos;

    private void Update()
    {
        if (isMoving) 
        { 
            currentPos = transform.position;

            var step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(currentPos, targetPos, step);

            if(currentPos == targetPos) 
            { 
                isMoving = false;
            }
        }
    }

    public void StartMoving(bool isUp)
    {
        if(!isMoving)
        {
            if(isUp && pos < positions.Length - 1) { pos++; }
            else if (!isUp && pos > 0) { pos--; }

            targetPos = positions[pos];
            isMoving = true;
        }
    }
}

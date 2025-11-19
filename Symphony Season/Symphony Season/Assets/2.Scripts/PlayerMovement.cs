using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Rigidbody2D rb2;
    [SerializeField] private float moveSpeed;
    private Vector3 moveDirection;

    [SerializeField] public InputActionReference move;
    [SerializeField] public InputActionReference interact;

    public bool isGridBased;

    private bool isMoving;
    private Vector3 origPos, targetPos;
    [SerializeField] private float timeToMove;

    private void Update()
    {
        if(isGridBased) 
        { 
            if(!isMoving) 
            {
                moveDirection = move.action.ReadValue<Vector3>();
                StartCoroutine(MovePlayer(moveDirection));
            }
        }
        if (!isGridBased)
        {
            moveDirection = move.action.ReadValue<Vector2>();
        }
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;

        float elapsedTime = 0;

        origPos = transform.position;
        targetPos = origPos + direction;

        while(elapsedTime < timeToMove) 
        { 
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime/timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
    }

    private void FixedUpdate()
    {
        if (!isGridBased)
        {
            rb.AddForce(new Vector3(moveDirection.x * moveSpeed, 0f, moveDirection.y * moveSpeed));
        }
    }

    private void OnEnable()
    {
        interact.action.started += Interact;
    }
    private void OnDisable()
    { //belangrijk omdat het anders 2 keer initialised
        interact.action.started -= Interact;
    }

    private void Interact(InputAction.CallbackContext obj) 
    {
        Debug.Log("Interacted");
    }
}

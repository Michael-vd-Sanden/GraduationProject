using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //WASD move
    [SerializeField] private float timeToMove;
    private bool isMoving;
    private Vector3 origPos, targetPos;
    private Vector3 moveDirection;

    [SerializeField] public InputActionReference moveWASD;
    [SerializeField] public InputActionReference interact;
    [SerializeField] public InputActionReference mouseMove;


    private void Update()
    {
        if(!isMoving) 
        {
            moveDirection = moveWASD.action.ReadValue<Vector3>();
            StartCoroutine(MovePlayer(moveDirection));
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


    //interact button
    private void OnEnable()
    {
        interact.action.started += Interact;
        mouseMove.action.started += MouseMove;
    }
    private void OnDisable()
    { //belangrijk omdat het anders 2 keer initialised
        interact.action.started -= Interact;
        mouseMove.action.started -= MouseMove;
    }

    private void Interact(InputAction.CallbackContext obj) 
    {
        Debug.Log("Interacted");
    }

    private void MouseMove(InputAction.CallbackContext obj)
    {
        Debug.Log("Supposed to move");
    }
}

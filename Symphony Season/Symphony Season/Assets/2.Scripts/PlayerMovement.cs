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

    private void Update()
    {
        moveDirection = move.action.ReadValue<Vector2>();

    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(moveDirection.x * moveSpeed, 0f, moveDirection.y * moveSpeed));
        //rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, 0f, moveDirection.y * moveSpeed);
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

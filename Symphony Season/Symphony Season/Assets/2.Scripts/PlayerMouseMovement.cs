using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMouseMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float sampleDistance = 0.5f;
   // [SerializeField] private Tilemap tilemap;
    [SerializeField] private Vector3 offset = new Vector3(0.5f, 0f, 0.5f);
    //[SerializeField] private Vector2 gridSize = new Vector2(1f, 1f);
    //[SerializeField] public InputActionReference moveMouse;
    [SerializeField] private NavMeshAgent agent;
    //private Vector3 origPos, targetPos;
    //private Vector3 moveDirection;

    private Vector3 screenPos, worldPos, gridPos;
    public GameObject testObj;
    public LayerMask layersToHit;

    private void Start()
    {
        agent.speed = moveSpeed;
    }

    private void Update()
    {
        if(Mouse.current.leftButton.isPressed) 
        {
            screenPos = Mouse.current.position.ReadValue();
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            if(Physics.Raycast(ray, out RaycastHit hitData, 100, layersToHit))
            {
                if (NavMesh.SamplePosition(hitData.point, out NavMeshHit navMeshHit, sampleDistance, NavMesh.AllAreas))
                {
                    worldPos = navMeshHit.position + offset;
                    gridPos = new Vector3(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y), Mathf.FloorToInt(worldPos.z));
                    //testObj.transform.position = gridPos;
                    agent.SetDestination(gridPos);
                    Debug.Log(gridPos.ToString());
                }
                else
                {
                    Debug.Log("not on navmesh");
                }
            }      
        }
        /*
        moveDirection = moveMouse.action.ReadValue<Vector3>();
        if(moveDirection != Vector3.zero ) 
        {
            Debug.Log(moveDirection.ToString());
            move(); 
        }*/
    }

    private void move(Vector3 direction)
    {
        agent.destination = direction;
        agent.Move(offset);
    }

    /*private void OnEnable()
    {
        moveMouse.action.started += MoveMouse;
    }
    private void OnDisable()
    { //belangrijk omdat het anders 2 keer initialised
        moveMouse.action.started -= MoveMouse;
    }

    private void MoveMouse(InputAction.CallbackContext obj)
    {
        Debug.Log("Supposed to move");
    }*/
}

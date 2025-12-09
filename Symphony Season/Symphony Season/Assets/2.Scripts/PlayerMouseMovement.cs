using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;
using TMPro;

public class PlayerMouseMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float sampleDistance = 0.5f;
    [SerializeField] private Vector3 offset = new Vector3(0.5f, 0f, 0.5f);
    [SerializeField] private Vector3 targetMargin = new Vector3(0.1f, 0f, 0.1f);
    [SerializeField] private bool isMoving = false;
    private Vector3 currentPos, destination;
    public NavMeshAgent agent;

    [SerializeField] public InputActionReference moveAction;

    private Vector3 screenPos, worldPos, gridPos;
    public LayerMask layersToHit;

    [SerializeField] private Vector2[] UIMask;

    public bool allowedToMove;
    public bool canBeOverUI;

    private void Start()
    {
        agent.speed = moveSpeed;
    }

    private void Update()
    {
        if(isMoving) 
        {
            currentPos = transform.position;
            if ((currentPos.x - targetMargin.x < destination.x && currentPos.x + targetMargin.x > destination.x)
                && (currentPos.z - targetMargin.z < destination.z && currentPos.z + targetMargin.z > destination.z))
            {
                transform.position = new Vector3(destination.x, currentPos.y, destination.z);
                isMoving = false;
            }
        }
    }

    private void castRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (canBeOverUI)
        {
            foreach (Vector2 pos in UIMask)
            {
                if (screenPos.x >= pos.x && screenPos.y <= pos.y)
                { //inside UIMask
                   // Debug.Log("in UIMask");
                        return;
                }
            }
        }

        if (Physics.Raycast(ray, out RaycastHit hitData, 100, layersToHit))
        {
            if (NavMesh.SamplePosition(hitData.point, out NavMeshHit navMeshHit, sampleDistance, NavMesh.AllAreas))
            {
                worldPos = navMeshHit.position + offset;
                gridPos = new Vector3(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y), Mathf.FloorToInt(worldPos.z));

                destination = gridPos;

                var path = new NavMeshPath();
                agent.CalculatePath(destination, path);
                switch (path.status)
                {
                    case NavMeshPathStatus.PathComplete:
                        isMoving = true;
                        agent.SetDestination(destination);
                        break;
                    default:
                        //Debug.Log("Can't move there");
                        break;
                }
            }
            else
            {
                //Debug.Log("not on navmesh");
                if(hitData.collider.CompareTag("Movable"))
                {
                    Debug.Log("hit block");
                    //ga naar closest point van block
                }
            }
        }
    }

    private void OnEnable()
    {
        moveAction.action.performed += Move;
    }

    private void OnDisable()
    {
        moveAction.action.performed -= Move;
    }

    public void MoveOutsideScript(Vector3 pos) 
    {
        destination = pos;
        isMoving = true;
        agent.SetDestination(pos);
    }

    private void Move(InputAction.CallbackContext obj)
    {
        screenPos = obj.ReadValue<Vector2>();
        if (allowedToMove) 
        {
            castRay();    
        }
    }
}

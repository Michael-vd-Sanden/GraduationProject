using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMouseMovement : MonoBehaviour
{
    [Header("-------------- Required Objects")]
    public NavMeshAgent agent;
    [SerializeField] public InputActionReference moveAction;
    public LayerMask layersToHit;
    [SerializeField] private Vector2[] UIMask;
    private PlayerButtonMovement playerButtonMove;

    [Header("-------------- Changeble Values")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float sampleDistance = 0.5f;
    [SerializeField] private Vector3 offset = new Vector3(0.5f, 0f, 0.5f);
    [SerializeField] private Vector3 targetMargin = new Vector3(0.1f, 0f, 0.1f);
    public bool isInMaze;
    public bool isMouseMovement;

    [Header("-------------- Background Values (do not change)")]
    public bool allowedToMove;
    public bool canBeOverUI;
    public bool isMoving = false;
    public bool isMovingLeft = false;
    [SerializeField] private Vector3 currentPos, destination;
    private Vector3 screenPos, worldPos, gridPos;

    private void Start()
    {
        agent.speed = moveSpeed;
        playerButtonMove= GetComponent<PlayerButtonMovement>();
    }

    private void Update()
    {
        if(isMoving) 
        {
            if(isInMaze) { allowedToMove= false; }
            currentPos = transform.position;
            if ((currentPos.x - targetMargin.x < destination.x && currentPos.x + targetMargin.x > destination.x)
                && (currentPos.z - targetMargin.z < destination.z && currentPos.z + targetMargin.z > destination.z))
            {
                transform.position = new Vector3(destination.x, currentPos.y, destination.z);
                isMoving = false;
                if(isInMaze) { allowedToMove= true; }
                if (!isMouseMovement && !isInMaze) { playerButtonMove.CheckPlayerDirections();}
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
                //Debug.Log(gridPos);
                destination = gridPos;

                CheckIfCanReachDestination();
            }
            else
            {
                //Debug.Log("not on navmesh");
                if(hitData.collider.CompareTag("Movable"))
                {
                    //Debug.Log("hit block");
                    //ga naar closest point van block
                }
            }
        }
    }

    private void CheckIfCanReachDestination()
    {
        var path = new NavMeshPath();
        agent.CalculatePath(destination, path);
        switch (path.status)
        {
            case NavMeshPathStatus.PathComplete:
                agent.SetDestination(destination);
                currentPos = transform.position;
                if(destination.x < currentPos.x || destination.z > currentPos.z)
                { isMovingLeft = true; }
                else { isMovingLeft = false; }
                isMoving = true;
                break;
            default:
                //Debug.Log("Can't move there");
                break;
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
        CheckIfCanReachDestination();
    }

    private void Move(InputAction.CallbackContext obj)
    {
        screenPos = obj.ReadValue<Vector2>();
        if (allowedToMove && !isInMaze && isMouseMovement) 
        {
            castRay();    
        }
    }
}

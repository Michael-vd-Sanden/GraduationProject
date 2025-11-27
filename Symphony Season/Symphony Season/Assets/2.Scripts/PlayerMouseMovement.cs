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
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] public InputActionReference moveAction;

    public Rigidbody rb;

    private Vector3 screenPos, worldPos, gridPos;
    public GameObject testObj;
    public LayerMask layersToHit;

    [SerializeField] private Vector2[] UIMask;

    [SerializeField] private bool isMobile;
    public bool allowedToMove; //when holding a block
    public bool canBeOverUI;

    //jitter fix
    [SerializeField] private bool checkMove = false;
    private Vector3 checkPos;
    private Vector3 moveMargin = new Vector3(0.1f, 0f, 0.1f);

    private void Start()
    {
        agent.speed = moveSpeed;
    }

    private void Update()
    {
        if (!isMobile)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                screenPos = Mouse.current.position.ReadValue();
                if(allowedToMove) { castRay(); }
            }
        }
        if(checkMove)
        {
            if ((transform.position.x + moveMargin.x >= checkPos.x && transform.position.x - moveMargin.x <= checkPos.x)
                && 
                (transform.position.z + moveMargin.z >= checkPos.z && transform.position.z - moveMargin.z <= checkPos.z))
            { AtDestination(); }
        }
        
    }

    private void AtDestination()
    {// at destination
        agent.isStopped = true;
        checkMove = false;
        //transform.position = new Vector3(checkPos.x, 1f, checkPos.z);
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
                    //Debug.Log("in UIMask");
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
                agent.isStopped = false;
                agent.SetDestination(gridPos);
                checkPos = gridPos;
                //checkMove = true;
            }
            else
            {
                Debug.Log("not on navmesh");
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

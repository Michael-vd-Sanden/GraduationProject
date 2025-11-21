using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMouseMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float sampleDistance = 0.5f;
    [SerializeField] private Vector3 offset = new Vector3(0.5f, 0f, 0.5f);
    [SerializeField] private NavMeshAgent agent;

    [SerializeField] public InputActionReference moveAction;

    private Vector3 screenPos, worldPos, gridPos;
    public GameObject testObj;
    public LayerMask layersToHit;

    [SerializeField] private bool isMobile;
    public bool allowedToMove;

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
    }

    private void castRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hitData, 100, layersToHit))
        {
            //check if ray hits the UI layer, otherwise move as normal
            if (hitData.transform.gameObject.layer == 3) //3 is int of Floor layer
            {
                if (NavMesh.SamplePosition(hitData.point, out NavMeshHit navMeshHit, sampleDistance, NavMesh.AllAreas))
                {
                    worldPos = navMeshHit.position + offset;
                    gridPos = new Vector3(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y), Mathf.FloorToInt(worldPos.z));
                    agent.SetDestination(gridPos);
                    Debug.Log(gridPos.ToString());
                }
                else
                {
                    Debug.Log("not on navmesh");
                }
            }
            else
            {
                Debug.Log(hitData.transform.gameObject.layer.ToString());
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

    private void Move(InputAction.CallbackContext obj)
    {
        screenPos = obj.ReadValue<Vector2>();
        if (allowedToMove) { castRay(); }
    }
}

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class VictoryTrigger : MonoBehaviour
{
    public UIToggles uiToggles;
    public TriggerSetter CurtainCloser;
    public MoveObject PlayerMover;
    public GameObject PlayerMat, playerObject;

    private PlayerInput PlayerInput;
    private NavMeshAgent NavMeshAgent;
    private PlayerMouseMovement PMM;


    private void Awake()
    {
        PlayerInput = playerObject.GetComponent<PlayerInput>();
        NavMeshAgent = playerObject.GetComponent<NavMeshAgent>();
        PMM = playerObject.GetComponent<PlayerMouseMovement>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            uiToggles.Victory();
            CurtainCloser.SetTrigger();
            PlayerInput.enabled = false;
            NavMeshAgent.enabled = false;
            PMM.enabled = false;
            PlayerMover.StartMoving(true);
            PlayerMat.GetComponent<MeshRenderer>().material.SetFloat("_IsMoving", 1f);
        }

    }
}

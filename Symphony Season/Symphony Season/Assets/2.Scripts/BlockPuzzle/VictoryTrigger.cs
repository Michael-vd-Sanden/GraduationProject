using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class VictoryTrigger : MonoBehaviour
{
    [SerializeField] private MoveUIToggles moveUIToggles;
    [SerializeField] private BPUiToggles bpUIToggles;
    [SerializeField] private TriggerSetter curtainCloser;
    [SerializeField] private MoveObject playerMover;
    [SerializeField] private NavMeshAgent playerAgent;
    [SerializeField] private PlayerMovement playerMovement;

    [SerializeField] private PlayerData playerData;

    private bool isMoving = false;

    private void Update()
    {
        if(isMoving) 
        { 
            playerData.currentPos = playerMover.transform.position;     //so the playersprite can follow the correct position
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            moveUIToggles.TurnOffDirections();   //turn off move arrows    
          
            curtainCloser.SetTrigger();          //close curtain
            bpUIToggles.Victory();               //victory screen active

            // TODO: 
            // Save the player score with JSONHandler

            isMoving = true;

            playerMovement.enabled = false;     //so it doesn't try to calculate position on navmesh
            playerAgent.enabled = false;        //so the player can move off the navmesh
            playerMover.StartMoving(true);
            playerData.isMoving = true;
            playerData.destination = playerMover.targetPos;
        }
    }
}

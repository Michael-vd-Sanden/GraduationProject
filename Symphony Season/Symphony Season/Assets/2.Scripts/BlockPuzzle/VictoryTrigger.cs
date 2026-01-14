using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    public UIToggles uiToggles;
    public TriggerSetter CurtainCloser;
    public MoveObject PlayerMover;
    public GameObject PlayerMat;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            uiToggles.Victory();
            CurtainCloser.SetTrigger();
            PlayerMover.StartMoving(true);
            PlayerMat.GetComponent<MeshRenderer>().material.SetFloat("_IsMoving", 1f);
        }

    }
}

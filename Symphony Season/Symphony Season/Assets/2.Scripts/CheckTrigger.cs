using UnityEngine;

public class CheckTrigger : MonoBehaviour
{
    private MoveBlockScript mScript;
    private void OnEnable()
    {
        mScript= GetComponentInParent<MoveBlockScript>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            mScript.EnteredTriggerInChild(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            mScript.ExitedTriggerInChild(other);
        }
    }
}

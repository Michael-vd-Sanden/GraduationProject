using UnityEngine;

public class CheckTrigger : MonoBehaviour
{
    private MoveBlockScript mScript;
    [SerializeField] private bool isFront;

    private void OnEnable()
    {
        mScript= GetComponentInParent<MoveBlockScript>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            mScript.EnteredTriggerInChild(other, isFront);
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

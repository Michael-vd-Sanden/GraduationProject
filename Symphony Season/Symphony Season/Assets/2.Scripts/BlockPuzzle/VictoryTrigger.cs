using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    public UIToggles uiToggles;
    public TriggerSetter CurtainCloser;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            uiToggles.Victory();
            CurtainCloser.SetTrigger();
        }

    }
}

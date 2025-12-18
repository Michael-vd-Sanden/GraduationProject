using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    public UIToggles uiToggles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            uiToggles.startVictory();
        }
    }
}

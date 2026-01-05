using UnityEngine;

public class OnOff : MonoBehaviour
{
    private bool SwitchState = true;
    public GameObject StateOne;
    public GameObject StateTwo;

    public void SWITCH()
    {
        if (SwitchState)
        {
            StateOne.SetActive(false);
            StateTwo.SetActive(true);
            SwitchState = false;
        }
        else
        {
            StateOne.SetActive(true);
            StateTwo.SetActive(false);
        }
    }
}


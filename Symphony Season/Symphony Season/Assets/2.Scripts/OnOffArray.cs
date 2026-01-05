using UnityEngine;

public class OnOffArray : MonoBehaviour
{
    private bool SwitchState = true;
    public GameObject[] StateOne;
    public GameObject[] StateTwo;
    public int ArrayLength;

    public void SWITCH()
    {
        if (SwitchState)
        {
            for(int i = 0; i < ArrayLength; i++)
            {
                StateOne[i].SetActive(false);
                StateTwo[i].SetActive(true);
                SwitchState = false;
            }

        } else
        {
            for (int i = 0; i < ArrayLength; i++)
            {
                StateOne[i].SetActive(true);
                StateTwo[i].SetActive(false);
                SwitchState = true;
            }
        }
    }
}


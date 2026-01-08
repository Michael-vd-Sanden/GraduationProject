using TMPro;
using UnityEngine;

public class LevelTextChanger : MonoBehaviour
{
    public GameObject ThisObject;
    public void LevelTextShift(int LevelIndex)
    {
        TextMeshPro.print("Floor " + LevelIndex);
    }
}

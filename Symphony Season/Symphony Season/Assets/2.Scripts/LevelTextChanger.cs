using TMPro;
using UnityEngine;

public class LevelTextChanger : MonoBehaviour
{
    private TMP_Text ThisText;
    public void Awake()
    {
        ThisText = GetComponent<TMP_Text>();
    }
    public void LevelTextShift(int LevelIndex)
    {
        ThisText.text = "FLOOR " + LevelIndex;
    }
}

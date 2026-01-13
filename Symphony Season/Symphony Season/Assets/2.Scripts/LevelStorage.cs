using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStorage : MonoBehaviour
{
    public bool HardMode = false;
    public Scene TestScene;
    public string[] Levels;
    public string[] HardModeLevels;
    public LevelIndex LevelIndex;
    
    public void HardModeShift()
    {
        if (!HardMode) { HardMode = true; }
        else if (HardMode) { HardMode = false; }
    }

    public void NextScene()
    {
        if (!HardMode)
        {
            SceneManager.LoadScene(Levels[LevelIndex.FloorIndex]);
        } else if (HardMode)
        {
            SceneManager.LoadScene(HardModeLevels[LevelIndex.FloorIndex]);
        }
    }
}

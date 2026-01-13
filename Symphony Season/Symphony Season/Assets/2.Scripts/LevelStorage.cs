using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStorage : MonoBehaviour
{
    public bool HardMode;
    public Scene[] Levels;
    public Scene[] HardModeLevels;
    public LevelIndex LevelIndex;

    public void HardModeShift()
    {
        if (!HardMode) { HardMode = true; }
        else { HardMode = false; }
    }

    public void ScenePlay()
    {
        string LevelName = Levels[LevelIndex.FloorIndex].name;
            SceneManager.LoadScene(LevelName);

    }
}

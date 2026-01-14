using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelStorage : MonoBehaviour
{
    public bool HardMode = false;
    public string[] Levels;
    public string[] HardModeLevels;
    public LevelIndex LevelIndex;
    public TriggerSetter CurtainCloser;

    private bool hasStarted;

    public void HardModeShift()
    {
        if (!HardMode) { HardMode = true; }
        else if (HardMode) { HardMode = false; }
    }

    public void StartNextScene()
    {
        if (!hasStarted) { StartCoroutine(NextScene()); }
    }

    private IEnumerator NextScene()
    {
        hasStarted = true;
        CurtainCloser.SetTrigger();
        yield return new WaitForSecondsRealtime(2.2f);
        if (!HardMode)
        {
            SceneManager.LoadScene(Levels[LevelIndex.FloorIndex]);
        }
        else if (HardMode)
        {
            SceneManager.LoadScene(HardModeLevels[LevelIndex.FloorIndex]);
        }
        hasStarted = false;
    }
}

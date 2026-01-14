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
    [SerializeField] private GameObject levelButtons;

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
        if (!HardMode && Levels[LevelIndex.FloorIndex].ToString() != "-")
        {
            levelButtons.SetActive(false);
            CurtainCloser.SetTrigger();
            yield return new WaitForSecondsRealtime(2.2f);
            SceneManager.LoadScene(Levels[LevelIndex.FloorIndex]);
        }
        else if (HardMode && HardModeLevels[LevelIndex.FloorIndex].ToString() != "-")
        {
            levelButtons.SetActive(false);
            CurtainCloser.SetTrigger();
            yield return new WaitForSecondsRealtime(2.2f);
            SceneManager.LoadScene(HardModeLevels[LevelIndex.FloorIndex]);
        }
        hasStarted = false;
    }
}

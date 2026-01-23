using UnityEngine;

public class PauzeScript : MonoBehaviour
{
    private SceneSwitching sceneSwitch;
    private TriggerSetter curtainTriggers;

    private void Awake()
    {
        sceneSwitch = FindFirstObjectByType<SceneSwitching>();
        curtainTriggers = FindFirstObjectByType<TriggerSetter>();
    }

    public void Pauze()
    {
        curtainTriggers.SetTrigger();
        //Time.timeScale = 0f;
    }

    public void UnPauze()
    {
        //Time.timeScale = 1.0f;
        curtainTriggers.SetTrigger();
    }

    public void MainMenu()
    {
        UnPauze();
        sceneSwitch.ChangeSceneName("MainMenu");
        sceneSwitch.ChangeScene();
    }
}

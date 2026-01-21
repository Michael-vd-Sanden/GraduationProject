using UnityEngine;

public class PauzeScript : MonoBehaviour
{
    private SceneSwitching sceneSwitch;

    private void Awake()
    {
        sceneSwitch = FindFirstObjectByType<SceneSwitching>();
    }

    public void Pauze()
    {
        Time.timeScale = 0f;
    }

    public void UnPauze()
    {
        Time.timeScale = 1.0f;
    }

    public void MainMenu()
    {
        UnPauze();
        sceneSwitch.ChangeSceneName("MainMenu");
        sceneSwitch.ChangeScene();
    }
}

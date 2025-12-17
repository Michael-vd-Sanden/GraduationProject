using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitching : MonoBehaviour
{
    private string newSceneName;

    private void Start()
    {
        newSceneName = "BP 1_Easy";
    }

    public void ChangeSceneName (string name)
    {
        newSceneName = name;
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(newSceneName);
    }
}

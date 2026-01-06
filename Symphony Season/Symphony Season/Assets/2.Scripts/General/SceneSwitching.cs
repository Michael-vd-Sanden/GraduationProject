using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitching : MonoBehaviour
{
    [Header("-------------- Changeble Values")]
    public string nextScene;

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

    public void NextScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}

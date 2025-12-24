using System.Collections;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    [Header("-------------- Required Objects")]
    [SerializeField] private GameObject player;
    private PlayerMouseMovement playerMove;
    [SerializeField] private UIToggles uiToggle;
    [SerializeField] private GameObject playerSpawn, arrow;
    [SerializeField] private GameObject[] tutorialLevels;
    //[Header("-------------- Changeble Values")]

    [Header("-------------- Background Values (do not change)")]
    public int tutorialLevel;
    private bool isRunning;

    private void Start()
    {
        playerMove = player.GetComponent<PlayerMouseMovement>();
        tutorialLevels[tutorialLevel].SetActive(true);
        StartCoroutine(Tut0());
    }

    private void SetAllLevelsInactive()
    {
        foreach (var level in tutorialLevels) 
        {
            level.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            NextTutorialLevel();
        }
    }

    private void NextTutorialLevel()
    {
        isRunning = true;
        if (tutorialLevel < tutorialLevels.Length -1)
        {
            tutorialLevel++;
        }
        else 
        { 
            uiToggle.startVictory();
            return;
        }
        tutorialLevels[tutorialLevel - 1].SetActive(false);
        tutorialLevels[tutorialLevel].SetActive(true);
        player.transform.position = playerSpawn.transform.position;
        playerMove.MoveOutsideScript(playerSpawn.transform.position);

        switch (tutorialLevel) 
        {
            case 1:
                break;
        }
        isRunning = false;
    }

    private IEnumerator Tut0()
    {//arrow points to exit
        arrow.SetActive(true);
        yield return new WaitForSecondsRealtime(5f);
        arrow.SetActive(false);
    }


}

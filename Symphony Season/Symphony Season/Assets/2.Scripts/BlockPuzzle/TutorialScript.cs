using System.Collections;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    [Header("-------------- Required Objects")]
    [SerializeField] private GameObject player;
    [SerializeField] private UIToggles UiToggles;
    [SerializeField] private BlockPuzzleManager manager;
    private PlayerMouseMovement playerMove;
    [SerializeField] private UIToggles uiToggle;
    [SerializeField] private GameObject cross;
    
    [Header("-------------- Changeble Values")]
    public int tutorialLevel;

    [Header("-------------- Background Values (do not change)")]
    private bool isRunning;

    private void Start()
    {
        playerMove = player.GetComponent<PlayerMouseMovement>();
        CheckTut();
    }

    private void CheckTut()
    {
        switch (tutorialLevel) 
        {
            case 1:
                StartCoroutine(TutMovement());
                break;
            case 2:
                break;
        }
    }

    private IEnumerator TutMovement()
    {//arrow points to exit
        //cross.SetActive(true);
        yield return new WaitForSecondsRealtime(5f);
        //cross.SetActive(false);
    }

    public void PressedHoldWithoutNotes()
    {
        UiToggles.TurnOffDirections();
        UiToggles.holdControl.SetActive(false);
        UiToggles.releaseControl.SetActive(true);

        manager.HoldBlock();

        if (manager.enteredBlocks.Count > 1)
        { UiToggles.switchControl.SetActive(true); }
        else
        { UiToggles.switchControl.SetActive(false); }
    }
}

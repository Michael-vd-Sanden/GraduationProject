using System.Collections;
using UnityEngine;

public class UIToggles : MonoBehaviour
{
    [Header("-------------- Required Objects")]
    [SerializeField] private BlockPuzzleManager manager;
    [SerializeField] private SceneSwitching sceneSwitching;
    [SerializeField]
    private GameObject holdControl, releaseControl, switchControl, pushLeftUpControl, pushLeftDownControl, pushRightUpControl, pushRightDownControl, victoryText,
        btnsSharp, btnsFlat, noteBtnsCanvas;
    [SerializeField] private GameObject[] noteBtnsObjects;

    public void EnteredTrigger()
    {
        holdControl.SetActive(true);
    }

    public void ExitedTrigger() 
    { 
        holdControl.SetActive(false);
        noteBtnsCanvas.SetActive(false);
        foreach(GameObject g in noteBtnsObjects) { g.SetActive(false); }
    }

    public void DeactivateNoteBtns()
    {
        noteBtnsCanvas.SetActive(false) ;
        foreach (GameObject g in noteBtnsObjects) { g.SetActive(false); }
    }

    public void SwitchSharpOrSflat()
    {
        btnsFlat.SetActive(!btnsFlat.activeSelf);
        btnsSharp.SetActive(!btnsSharp.activeSelf);
    }

    public void TurnOffDirections()
    {
        pushLeftDownControl.SetActive(false);
        pushLeftUpControl.SetActive(false);
        pushRightDownControl.SetActive(false);
        pushRightUpControl.SetActive(false);
    }

    public void PressedHoldBtn()
    {
        TurnOffDirections();
        holdControl.SetActive(false);
        releaseControl.SetActive(true);
        noteBtnsCanvas.SetActive(true);
        foreach (GameObject g in noteBtnsObjects) { g.SetActive(true); }
        manager.HoldBlock();

        if (manager.enteredBlocks.Count > 1)
        { switchControl.SetActive(true); }
        else
        { switchControl.SetActive(false); }
    }

    public void ActivateBlockDirections()
    {
        TurnOffDirections();

        MoveBlockScript block = manager.currentSelectedBlock;
        if (block.isRightDirection)
        { //right up direction
            if (block.upAllowed)
            { pushRightUpControl.SetActive(true); }
            if (block.downAllowed)
            { pushLeftDownControl.SetActive(true); }      
        }
        else
        {//left up direction
            if (block.upAllowed)
            { pushLeftUpControl.SetActive(true); }
            if (block.downAllowed)
            { pushRightDownControl.SetActive(true); }
        }

        if (manager.enteredBlocks.Count > 1)
        { switchControl.SetActive(true); }
        else
        { switchControl.SetActive(false); }
    }

    public void ActivatePlayerDirections(string direction, bool active)
    {
        switch (direction)
        {
            case "LeftUp":
                if (active) { pushLeftUpControl.SetActive(true); }
                else { pushLeftUpControl.SetActive(false); }
                break;
            case "RightUp":
                if (active) { pushRightUpControl.SetActive(true); }
                else { pushRightUpControl.SetActive(false); }
                break;
            case "LeftDown":
                if (active) { pushLeftDownControl.SetActive(true); }
                else { pushLeftDownControl.SetActive(false); }
                break;
            case "RightDown":
                if (active) { pushRightDownControl.SetActive(true); }
                else { pushRightDownControl.SetActive(false); }
                break;
        }   
    }

    public void PressedPushBtn(string direction) //miss geen button, maar een drag?
    {
        if (manager.currentSelectedBlock != null)
        { manager.currentSelectedBlock.MoveBlock(direction); }
    }

    public void PressedSwitchBtn()
    {
        //Debug.Log("pressed switch");
        pushLeftUpControl.SetActive(false);
        pushLeftDownControl.SetActive(false);
        pushRightDownControl.SetActive(false);
        pushRightUpControl.SetActive(false);
        noteBtnsCanvas.SetActive(true);
        foreach (GameObject g in noteBtnsObjects) { g.SetActive(true); }
        manager.SwitchBlock();
    }

    public void PressedReleaseBtn()
    {
        pushLeftUpControl.SetActive(false);
        pushLeftDownControl.SetActive(false);
        pushRightDownControl.SetActive(false);
        pushRightUpControl.SetActive(false);
        releaseControl.SetActive(false);
        switchControl.SetActive(false);
        noteBtnsCanvas.SetActive(false);
        foreach (GameObject g in noteBtnsObjects) { g.SetActive(false); }
        holdControl.SetActive(true);

        manager.LetGoOfBlock();
    }

    public void PressedNoteBtn(string note)
    {
        manager.noteSelected = note;
    }

    public void startVictory()
    { StartCoroutine(Victory()); }

    private IEnumerator Victory()
    {
        victoryText.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        sceneSwitching.ChangeSceneName("MainMenu");
        sceneSwitching.ChangeScene();
    }
}

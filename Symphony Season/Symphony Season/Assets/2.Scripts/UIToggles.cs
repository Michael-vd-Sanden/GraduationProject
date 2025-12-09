using System.Collections;
using UnityEngine;

public class UIToggles : MonoBehaviour
{
    [SerializeField] private GameObject holdControl, releaseControl, switchControl, pushLeftUpControl, pushLeftDownControl, pushRightUpControl, pushRightDownControl, victoryText,
        btnsSharp, btnsFlat, noteBtns;
    [SerializeField] private BlockPuzzleManager manager;
    [SerializeField] private SceneSwitching sceneSwitching;

    public void EnteredTrigger()
    {
        holdControl.SetActive(true);
    }

    public void ExitedTrigger() 
    { 
        holdControl.SetActive(false);
        noteBtns.SetActive(false);
    }

    public void DeactivateNoteBtns()
    {
        noteBtns.SetActive(false) ;
    }

    public void SwitchSharpOrSflat()
    {
        btnsFlat.SetActive(!btnsFlat.activeSelf);
        btnsSharp.SetActive(!btnsSharp.activeSelf);
    }

    public void PressedHoldBtn()
    {
        holdControl.SetActive(false);
        releaseControl.SetActive(true);
        noteBtns.SetActive(true);
        manager.HoldBlock(); 
    }

    public void ActivateDirections()
    {
        pushLeftDownControl.SetActive(false);
        pushLeftUpControl.SetActive(false);
        pushRightDownControl.SetActive(false);
        pushRightUpControl.SetActive(false);

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

        if(manager.enteredBlocks.Count > 1) 
        { switchControl.SetActive(true);  }
        else
        { switchControl.SetActive(false); }
    }

    public void PressedPushBtn(string direction) //miss geen button, maar een drag?
    {
        manager.currentSelectedBlock.MoveBlock(direction);
    }

    public void PressedSwitchBtn()
    {
        //Debug.Log("pressed switch");
        noteBtns.SetActive(true);
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
        noteBtns.SetActive(false);
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

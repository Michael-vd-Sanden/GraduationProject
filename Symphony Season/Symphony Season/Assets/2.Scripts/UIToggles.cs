using System.Collections.Generic;
using UnityEngine;

public class UIToggles : MonoBehaviour
{
    [SerializeField] private GameObject holdControl, releaseControl, switchControl, leftUpControl, leftDownControl,rightUpControl, rightDownControl;
    private List<bool> isRightDirection;
    [SerializeField] private List<MoveBlockScript> enteredBlocks;
    public MoveBlockScript selectedBlock;
    private BlockPuzzleManager manager;

    private void Awake()
    {
        manager = GetComponent<BlockPuzzleManager>();
    }

    public void EnteredTrigger(MoveBlockScript block) //entered trigger of this block
    {
        holdControl.SetActive(true);
        enteredBlocks.Add(block);
        isRightDirection.Add(block.isRightDirection);
    }

    public void ExitedTrigger(MoveBlockScript block) 
    { 
        holdControl.SetActive(false);
        enteredBlocks.Remove(block);
        isRightDirection.Remove(block.isRightDirection);
    }

    public void PressedHoldBtn()
    {
        holdControl.SetActive(false);
        releaseControl.SetActive(true);
        selectedBlock.HoldBlock();
        //check dist to walls
        selectedBlock.CheckIfAllowedToMove();

        //check if doubles
        if(manager.multipleActiveTriggers)
        { switchControl.SetActive(true);}
    }

    public void ActivateDirection(bool[] allowed)
    {
        //reset
        rightUpControl.SetActive(false);
        leftDownControl.SetActive(false);
        leftUpControl.SetActive(false);
        rightDownControl.SetActive(false);

        //set
        if (selectedBlock.isRightDirection)
        { //right up & left down
            if (allowed[0])
            { rightUpControl.SetActive(true); }
            if (allowed[1])
            { leftDownControl.SetActive(true); }
        }
        else
        {//left up & right down
            if (allowed[0])
            { leftUpControl.SetActive(true); }
            if (allowed[1])
            { rightDownControl.SetActive(true); }
        }
    }

    public void PressedPushBtn(string direction) //miss geen button, maar een drag?
    {
        if(enteredBlocks == null)
        { Debug.Log("block is null"); }
        selectedBlock.MoveBlock(direction);
    }

    public void PressedReleaseBtn()
    {
        rightUpControl.SetActive(false);
        leftDownControl.SetActive(false);
        leftUpControl.SetActive(false);
        rightDownControl.SetActive(false);
        releaseControl.SetActive(false);
        switchControl.SetActive(false);
        holdControl.SetActive(true);
        if (enteredBlocks != null)
        { selectedBlock.LetGoOfBlock(); }
    }
}

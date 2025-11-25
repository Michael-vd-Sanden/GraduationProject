using UnityEngine;

public class UIToggles : MonoBehaviour
{
    [SerializeField] private GameObject holdControl, pushLeftControl, pushRightControl;
    private bool isRightDirection;
    private MoveBlockScript currentSelectedBlock;

    public void EnteredTrigger(MoveBlockScript block)
    {
        holdControl.SetActive(true);
        currentSelectedBlock = block;
        isRightDirection = currentSelectedBlock.isRightDirection;
    }

    public void ExitedTrigger() 
    { 
        holdControl.SetActive(false);
        currentSelectedBlock = null;
    }

    public void PressedHoldBtn()
    {
        holdControl.SetActive(false);
        currentSelectedBlock.HoldBlock();

        if(isRightDirection) 
        { //right up direction
            pushRightControl.SetActive(true);
        }
        else
        {//left up direction
            pushLeftControl.SetActive(true);
        }
    }

    public void PressedPushBtn(string direction) //miss geen button, maar een drag?
    {
        currentSelectedBlock.MoveBlock(direction);
    }

    public void PressedReleaseBtn()
    {
        pushLeftControl.SetActive(false);
        pushRightControl.SetActive(false);
        holdControl.SetActive(true);
        currentSelectedBlock.LetGoOfBlock();
    }
}

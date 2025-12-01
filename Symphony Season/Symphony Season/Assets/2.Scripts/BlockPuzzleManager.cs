using System.Collections.Generic;
using UnityEngine;

public class BlockPuzzleManager : MonoBehaviour
{
    [SerializeField] private PlayerMouseMovement playerMovement;
    [SerializeField] private UIToggles uiToggle;

    public List<MoveBlockScript> enteredBlocks;
    [SerializeField] private int selectedBlockIndex = 0;
    public MoveBlockScript currentSelectedBlock;

    public int layer;
    private int layerAsLayerMask;

    private void Awake()
    {
        layerAsLayerMask = (1 << layer);
    }

    public void EnteredTrigger(MoveBlockScript block)
    {
        enteredBlocks.Add(block);
        if (enteredBlocks.Count > 0) 
        { 
            playerMovement.canBeOverUI= true;
            uiToggle.EnteredTrigger();
        }     
    }
    public void ExitedTrigger(MoveBlockScript block)
    {
        enteredBlocks.Remove(block);
        if (enteredBlocks.Count == 0) 
        { 
            playerMovement.canBeOverUI= false;
            uiToggle.ExitedTrigger();
        }
    }

    public void HoldBlock()
    {
        if(enteredBlocks.Count > 0) 
        {
            currentSelectedBlock = enteredBlocks[selectedBlockIndex];
            playerMovement.allowedToMove = false;
            currentSelectedBlock.objectAbleToMove = true;
            CheckIfAllowedToMove();
        }
    }
    public void SwitchBlock()
    {
        LetGoOfBlock();
        Debug.Log(enteredBlocks.Count.ToString());
        if(enteredBlocks.Count -1 > selectedBlockIndex)
        {
            selectedBlockIndex++;
            currentSelectedBlock = enteredBlocks[selectedBlockIndex];
        }
        else
        {
            selectedBlockIndex = 0;
            currentSelectedBlock = enteredBlocks[selectedBlockIndex];
        }
        HoldBlock();
    }
    public void LetGoOfBlock()
    {
        currentSelectedBlock.objectAbleToMove = false;
        currentSelectedBlock = null;
        playerMovement.allowedToMove = true;
    }

    public void CheckIfAllowedToMove()
    {
        Transform b = currentSelectedBlock.transform;
        for (int check = 0; check < 2; check++)
        {
            Vector3 rayDirect;
            if (check == 0)
            { rayDirect = b.forward; }
            else
            { rayDirect = -b.forward; }

            RaycastHit hit;
            if (Physics.Raycast(b.position, rayDirect, out hit, 3f, layerAsLayerMask))
            {
                Debug.DrawRay(b.position, rayDirect * hit.distance, Color.green, 2f);
                //Debug.Log("object hit: " + hit.transform.name.ToString());

                int allowedDistance;
                if ((currentSelectedBlock.playerIsFront && rayDirect == b.forward) || (!currentSelectedBlock.playerIsFront && rayDirect == -b.forward))
                {// player is in front and moving that direction
                    allowedDistance = currentSelectedBlock.playerDistance;
                }
                else { allowedDistance = currentSelectedBlock.wallDistance; }
                Debug.Log("allowed distance: " + allowedDistance.ToString());
                Debug.Log("hit distance: " + hit.distance.ToString());

                if (hit.distance <= allowedDistance)
                {//too close
                    //Debug.Log("too close to move");
                    if(check == 0) { currentSelectedBlock.upAllowed = false; }
                    else { currentSelectedBlock.downAllowed = false; }
                }
                else
                {// able to move
                    if(check == 0) { currentSelectedBlock.upAllowed = true; }
                    else { currentSelectedBlock.downAllowed = true; }   
                }
            }
            else
            {//able to move
                Debug.DrawRay(b.position, rayDirect * 3f, Color.red, 2f);
                if (check == 0) { currentSelectedBlock.upAllowed = true; }
                else { currentSelectedBlock.downAllowed = true; }
            }
        }
        uiToggle.ActivateDirections();
        if(enteredBlocks.Count == 1) { selectedBlockIndex = 0; }
    }
}

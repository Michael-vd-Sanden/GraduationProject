using System.Collections.Generic;
using UnityEngine;

public class BlockPuzzleManager : MonoBehaviour
{
    [Header("-------------- Required Objects")]
    [SerializeField] private PlayerMouseMovement playerMovement;
    [SerializeField] private UIToggles uiToggle;
    [SerializeField] private AudioPlayer audioPlayer;
    [SerializeField] private PlayerFollower playerFollow;
    public PlayerButtonMovement playerBtnMove;

    [Header("-------------- Changeble Values")]
    public int hitLayer;
    private int layerAsLayerMask;

    [Header("-------------- Background Values (do not change)")]
    public List<MoveBlockScript> enteredBlocks;
    [SerializeField] private int selectedBlockIndex = 0;
    public MoveBlockScript currentSelectedBlock;
    public string currentBlockNote;
    public string noteSelected; //which note btn was pressed
    public Material[] colourMaterials;
    private bool isCheckingForNotes = false;

    private void Awake()
    {
        layerAsLayerMask = (1 << hitLayer);
    }

    private void Update()
    {
        if(isCheckingForNotes) 
        {
            if(currentSelectedBlock== null) { isCheckingForNotes = false; return; }

            if (noteSelected == currentBlockNote) //goede noot
            {
                //audioPlayer.PlayEffect(noteSelected); Al in button
                playerMovement.allowedToMove = false;
                currentSelectedBlock.objectAbleToMove = true;
                currentSelectedBlock.noteNotification.SetActive(true);
                currentSelectedBlock.questionNotification.SetActive(false);
                CheckIfAllowedToMove();
                noteSelected = null;
                uiToggle.DeactivateNoteBtns();
                isCheckingForNotes= false;
                return;
            }
            if(!string.IsNullOrEmpty(noteSelected) && noteSelected != currentBlockNote) //foute noot
            {
                //play some sort of sound
                audioPlayer.PlayEffect("Wrong");
                Debug.Log("fout");
                noteSelected = null;
            }
        }
    }

    public void EnteredTrigger(MoveBlockScript block)
    {
        enteredBlocks.Add(block);
        if (enteredBlocks.Count > 0) 
        { 
            playerMovement.canBeOverUI= true;
        }   
        if(enteredBlocks.Count == 1)
        {
            uiToggle.EnteredTrigger();
        }
    }
    public void ExitedTrigger(MoveBlockScript block)
    {
        enteredBlocks.Remove(block);
        if (enteredBlocks.Count == 0) 
        {
            selectedBlockIndex = 0;
            playerMovement.canBeOverUI= false;
            uiToggle.ExitedTrigger();
        }
    }

    public void HoldBlock()
    {
        if(enteredBlocks.Count > 0) 
        {
            currentSelectedBlock = enteredBlocks[selectedBlockIndex];
            currentBlockNote = currentSelectedBlock.blockNote;
            currentSelectedBlock.questionNotification.SetActive(true);
            isCheckingForNotes = true;
            noteSelected = null;
            playerFollow.ToggleHolding(1f);
        }
    }
    public void SwitchBlock()
    {
        LetGoOfBlock();
        //Debug.Log(enteredBlocks.Count.ToString());
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
        if (currentSelectedBlock != null)
        {
            currentSelectedBlock.questionNotification.SetActive(false);
            currentSelectedBlock.noteNotification.SetActive(false);
            currentSelectedBlock.objectAbleToMove = false;
            currentSelectedBlock = null;
            playerFollow.ToggleHolding(0f);
        }
        currentBlockNote = null;
        playerMovement.allowedToMove = true;
        if(!playerMovement.isMouseMovement)
        { playerBtnMove.CheckPlayerDirections(); }
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
                Debug.DrawRay(b.position, rayDirect * hit.distance, Color.red, 2f);
                //Debug.Log("object hit: " + hit.transform.name.ToString());

                float allowedDistance;
                if ((currentSelectedBlock.playerIsFront && rayDirect == b.forward) || (!currentSelectedBlock.playerIsFront && rayDirect == -b.forward))
                {// player is in front and moving that direction
                    allowedDistance = currentSelectedBlock.playerDistance;
                }
                else { allowedDistance = currentSelectedBlock.wallDistance; }
                //Debug.Log("allowed distance: " + allowedDistance.ToString());
                //Debug.Log("hit distance: " + hit.distance.ToString());

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
                Debug.DrawRay(b.position, rayDirect * 3f, Color.green, 2f);
                if (check == 0) { currentSelectedBlock.upAllowed = true; }
                else { currentSelectedBlock.downAllowed = true; }
            }
        }
        uiToggle.ActivateBlockDirections();
        if(enteredBlocks.Count == 1) { selectedBlockIndex = 0; }
    }
}

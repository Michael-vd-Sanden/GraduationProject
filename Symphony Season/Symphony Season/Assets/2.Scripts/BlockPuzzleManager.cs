using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPuzzleManager : MonoBehaviour
{
    [SerializeField] private float waitingSeconds;
    [SerializeField] private PlayerMouseMovement player;
    private int selectedTrigger = 0;
    public bool multipleActiveTriggers;

    public CheckTrigger[] triggers;
    public List<CheckTrigger> activeTriggers;
    private MoveBlockScript selectedTriggerBlock;


    void Start()
    {
        //StartCoroutine(startAnimation());
    }

    private IEnumerator startAnimation()
    {
        player.rb.useGravity = true;
        player.rb.isKinematic = false;
        yield return new WaitForSecondsRealtime(waitingSeconds);
        player.rb.isKinematic = true;
        player.rb.useGravity = false;
        yield return null;
    }

    public void selectFirstTrigger() //when hold btn pressed
    {
        selectedTrigger = 0;
    }
    public void UnSelectTrigger() //when release btn pressed
    {
        activeTriggers.Clear();
    }

    public void checkDoubleTriggers() //when hold btn pressed & when moved
    {
        int i = 0;
        activeTriggers.Clear();
        foreach (CheckTrigger trigger in triggers) 
        {
            if(trigger.insideThis)
            {
                activeTriggers.Add(trigger);
                i ++;
            }
        }
        if(activeTriggers.Count > 1) { multipleActiveTriggers= true; }
        else { multipleActiveTriggers= false; }
        selectedTriggerBlock = activeTriggers[selectedTrigger].GetBlock();
        activeTriggers[selectedTrigger].SelectedTrigger(selectedTriggerBlock);
    }

    public void switchTrigger() //when switch btn pressed
    {
        activeTriggers[selectedTrigger].UnSelectedTrigger();
        if (activeTriggers.Count <= selectedTrigger)
        {
            selectedTrigger++;
        }
        else
        {
            selectedTrigger = 0;
        }
        selectedTriggerBlock = activeTriggers[selectedTrigger].GetBlock();
        activeTriggers[selectedTrigger].SelectedTrigger(selectedTriggerBlock);
    }
}

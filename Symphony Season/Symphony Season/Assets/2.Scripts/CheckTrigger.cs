using UnityEngine;

public class CheckTrigger : MonoBehaviour
{
    private MoveBlockScript mScript;
    private PlayerMouseMovement player;
    private BlockPuzzleManager manager;
    private UIToggles uiToggles;
    [SerializeField] private bool isFront;

    public bool insideThis = false;
    private Collider playerCollider;

    private void OnEnable()
    {
        mScript = GetComponentInParent<MoveBlockScript>();
        player = FindFirstObjectByType<PlayerMouseMovement>();
        manager = FindFirstObjectByType<BlockPuzzleManager>();
        uiToggles = FindFirstObjectByType<UIToggles>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            insideThis = true;
            playerCollider = other;
            //mScript.EnteredTriggerInChild(playerCollider, isFront);
            uiToggles.EnteredTrigger(mScript);

            Debug.Log("enter");
            /*
            Debug.Log("double enter? " + manager.checkDoubleTriggers().ToString());
            if (player.allowedToMove && !manager.checkDoubleTriggers())
            {
                Debug.Log("Entered: " + this.name.ToString());
                insideThis = true;
                mScript.EnteredTriggerInChild(other, isFront);
            }*/
        }
    }

    public MoveBlockScript GetBlock()
    {
        return mScript;
    }

    public void SelectedTrigger(MoveBlockScript block)
    {
        uiToggles.selectedBlock = block;
        //Debug.Log("Selected: " + this.name.ToString() + " trigger");
        //mScript.EnteredTriggerInChild(playerCollider, isFront);
    }
    public void UnSelectedTrigger()
    {
        uiToggles.selectedBlock = null;
       // Debug.Log("Unselected: " + this.name.ToString() + " trigger");
        //mScript.ExitedTriggerInChild(playerCollider);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            insideThis = false;
            playerCollider = other;
            //mScript.ExitedTriggerInChild(playerCollider);
            uiToggles.ExitedTrigger(mScript);

            Debug.Log("Exit");
            /*
            Debug.Log("holding? " + insideThis.ToString());
            if (player.allowedToMove && insideThis && manager.checkDoubleTriggers())
            {
                Debug.Log("Exited: " + this.name.ToString());
                mScript.ExitedTriggerInChild(other);
                insideThis = false;
            }*/
        }
    }
}

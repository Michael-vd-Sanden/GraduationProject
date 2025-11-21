using UnityEngine;

public class MoveBlockScript : MonoBehaviour
{
    [SerializeField] private UIToggles uiToggle;
    [SerializeField] private PlayerMouseMovement playerMovement;
    private bool objectAbleToMove;
    public bool isRightDirection; //set for every object

    private Vector3 currentPos, targetPos;

    private void Awake()
    {
        uiToggle = FindFirstObjectByType(typeof(UIToggles)) as UIToggles;
        playerMovement = FindFirstObjectByType(typeof(PlayerMouseMovement)) as PlayerMouseMovement;
    }

    public void EnteredTriggerInChild(Collider c)
    {
        Debug.Log(c.name.ToString() + " detected");
        uiToggle.EnteredTrigger(this);
    }

    public void ExitedTriggerInChild(Collider c)
    {
        Debug.Log(c.name.ToString() + " left");
        uiToggle.ExitedTrigger();
    }

    public void HoldBlock()
    {
        playerMovement.allowedToMove = false;
        objectAbleToMove = true;
    }

    public void MoveBlock(string direction)
    {//move 1 space
        currentPos = this.gameObject.transform.position;
        if (objectAbleToMove)
        {
            switch (direction)
            {
                case "RightUp":
                    targetPos = currentPos + new Vector3(0f, 0f, 0f);
                    break;
                case "LeftUp":
                    targetPos = currentPos + new Vector3(0f, 0f, 0f);
                    break;
                case "RightDown":
                    targetPos = currentPos + new Vector3(0f, 0f, 0f);
                    break;
                case "LeftDown":
                    targetPos = currentPos + new Vector3(0f, 0f, 0f);
                    break;
            }
        }
        this.gameObject.transform.position = targetPos;
    }

    public void LetGoOfBlock()
    {
        objectAbleToMove = false;
        playerMovement.allowedToMove = true;
    }

}

using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerButtonMovement : MonoBehaviour
{
    [Header("-------------- Required Objects")]
    private PlayerMouseMovement pMovement;
    private BlockPuzzleManager manager;
    private UIToggles UIToggles;
    private Vector3 playerDestination, currentPosition;
    private Transform globalRoot;

    [Header("-------------- Changeble Values")]
    public int layer;
    private int layerAsLayerMask;

    [Header("-------------- Background Values (do not change)")]
    [SerializeField] private bool isPressingMove;
    private string moveDirection;

    private void Awake()
    {
        pMovement = GetComponent<PlayerMouseMovement>();
        manager = FindFirstObjectByType<BlockPuzzleManager>();
        UIToggles= FindFirstObjectByType<UIToggles>();
        globalRoot = GameObject.FindGameObjectWithTag("GlobalRoot").transform;
        layerAsLayerMask = (1 << layer);
    }

    private void Start()
    {
        CheckPlayerDirections();
    }

    private void Update()
    {
        if(isPressingMove) 
        {
            MovePlayer();
        }
    }

    public void MovePlayer()
    {
        if (manager.currentSelectedBlock == null && !pMovement.isMoving && !pMovement.isMouseMovement)
        {
            Vector3 temp = new Vector3(0f, 0f, 0f);
            currentPosition = transform.position;
            switch (moveDirection)
            {
                case "LeftUp":
                    temp = new Vector3(0f, 0f, 1f);
                    break;
                case "RightUp":
                    temp = new Vector3(1f, 0f, 0f);
                    break;
                case "LeftDown":
                    temp = new Vector3(-1f, 0f, 0f);
                    break;
                case "RightDown":
                    temp = new Vector3(0f, 0f, -1f);
                    break;
            }
            playerDestination = currentPosition + temp;
            pMovement.MoveOutsideScript(playerDestination);   
        }
    }

    public void onPressMove(string direction)
    {
        if (manager.currentSelectedBlock == null)
        {
            moveDirection = direction;
            isPressingMove = true;
        }
        else
        {
            manager.currentSelectedBlock.moveDirection = direction;
            manager.currentSelectedBlock.isPressingBlockMove = true;
        }
    }

    public void onReleaseMove()
    {
        if (manager.currentSelectedBlock == null)
        {
            isPressingMove = false;
        }
        else
        {
            manager.currentSelectedBlock.isPressingBlockMove = false;
        }
    }

    public void CheckPlayerDirections()
    {
        if (manager.currentSelectedBlock == null && !pMovement.isMouseMovement)
        {

            Transform t = globalRoot;
            Vector3 playerPos = transform.position + new Vector3(0f, -0.5f, 0f);
            Vector3 rayDirect;
            bool able;

            for (int check = 0; check < 4; check++)
            {
                
                switch (check)
                {
                    case 0:
                        rayDirect = t.forward;
                        break;
                    case 1:
                        rayDirect = t.right;
                        break;
                    case 2:
                        rayDirect = -t.right;
                        break;
                    case 3:
                        rayDirect = -t.forward;
                        break;
                    default:
                        rayDirect = t.forward;
                        break;
                }

                RaycastHit hit;
                if (Physics.Raycast(playerPos, rayDirect, out hit, 3f, layerAsLayerMask))
                {
                     
                    if (hit.distance <= 1f)
                    {//too close
                        //Debug.Log("hit " + hit.collider.name);
                        //Debug.DrawRay(playerPos, rayDirect * 2f, Color.red, 2f);
                        able = false;
                    }
                    else 
                    {
                        //Debug.DrawRay(playerPos, rayDirect * 2f, Color.green, 2f);
                        able = true; 
                    }   
                }
                else 
                {
                    //Debug.DrawRay(playerPos, rayDirect * 2f, Color.green, 2f);
                    able = true; 
                }

                switch (check)
                {
                    case 0:
                        UIToggles.ActivatePlayerDirections("LeftUp", able);
                        break;
                    case 1:
                        UIToggles.ActivatePlayerDirections("RightUp", able);
                        break;
                    case 2:
                        UIToggles.ActivatePlayerDirections("LeftDown", able);
                        break;
                    case 3:
                        UIToggles.ActivatePlayerDirections("RightDown", able);
                        break;
                }
            }
        }
    }
}

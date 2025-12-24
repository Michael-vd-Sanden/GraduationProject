using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MoveBlockScript : MonoBehaviour
{
    [Header("-------------- Required Objects")]
    [SerializeField] private MeshRenderer colourBlockRenderer;
    [SerializeField] private MeshRenderer colourQuestionRenderer, colourNoteRenderer;
    public GameObject questionNotification, noteNotification;
    private Material colourMaterial;
    private BlockPuzzleManager manager;
    private ColourChanger colourChanger;

    [Header("-------------- Changeble Values")]
    public float playerDistance;
    public float wallDistance;
    public bool isRightDirection; //set for every object
    public string blockNote;
    [SerializeField] private int materialInArray;

    [Header("-------------- Background Values (do not change)")]
    //sets in awake
    [SerializeField] private PlayerMouseMovement playerMovement;
    //background values
    public bool objectAbleToMove;
    public bool upAllowed;
    public bool downAllowed;
    //positioning
    private Vector3 objectCurrentPos, objectTargetPos, playerTargetPos, playerCurrentPos;
    [SerializeField] private bool isMoving;
    public bool playerIsFront; //on which side the player is (true = front, false = back)
    //smooth movement
    [SerializeField] AnimationCurve stepEase = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
    [SerializeField] AnimationCurve stepHeightShape = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepDuration = 0.3f;
    private float stepTime;

    private void Awake()
    {
        colourChanger = FindFirstObjectByType<ColourChanger>();
        ChangeColourBasedOnNote();
        manager = FindFirstObjectByType<BlockPuzzleManager>();
        playerMovement = FindFirstObjectByType<PlayerMouseMovement>();
        isMoving = false;
    }

    private void Update()
    {
        if (isMoving) 
        {
            Move();
        }
    }

    public void EnteredTriggerInChild(Collider c, bool isFront)
    {
        //Debug.Log(c.name.ToString() + " detected");
        playerIsFront = isFront;
        manager.EnteredTrigger(this);
    }

    public void ExitedTriggerInChild(Collider c)
    {
        //Debug.Log(c.name.ToString() + " left");
        manager.ExitedTrigger(this);
    }

    public void MoveBlock(string direction)
    {//move 1 space
        //Debug.Log("pushed " + direction.ToString());
        objectCurrentPos = this.gameObject.transform.position;
        playerCurrentPos = playerMovement.transform.position;
        if (objectAbleToMove && !isMoving)
        {
            stepTime = 0f;
            switch (direction)
            {
                case "RightUp":
                    objectTargetPos = objectCurrentPos + new Vector3(1f, 0f, 0f);
                    playerTargetPos = playerCurrentPos + new Vector3(1f, 0f, 0f);
                    break;
                case "LeftUp":
                    objectTargetPos = objectCurrentPos + new Vector3(0f, 0f, 1f);
                    playerTargetPos = playerCurrentPos + new Vector3(0f, 0f, 1f);
                    break;
                case "RightDown":
                    objectTargetPos = objectCurrentPos + new Vector3(0f, 0f, -1f);
                    playerTargetPos = playerCurrentPos + new Vector3(0f, 0f, -1f);
                    break;
                case "LeftDown":
                    objectTargetPos = objectCurrentPos + new Vector3(-1f, 0f, 0f);
                    playerTargetPos = playerCurrentPos + new Vector3(-1f, 0f, 0f);
                    break;
            }
            isMoving = true; 
        }
    }

    private void Move()
    {
        stepTime += Time.deltaTime;
        float progress = stepEase.Evaluate(stepTime / stepDuration);

        Vector3 newPos = Vector3.Lerp(objectCurrentPos, objectTargetPos, progress);
        newPos.y += stepHeight * stepHeightShape.Evaluate(progress);

        //Debug.Log("targetPos: " + newPos.ToString());
        playerMovement.MoveOutsideScript(playerTargetPos);
        this.gameObject.transform.position = newPos;
        if (progress >= 1f)
        {
            isMoving = false;
            manager.CheckIfAllowedToMove();
        }
    }

    private void ChangeColourBasedOnNote()
    {
        colourMaterial = colourChanger.ChangeColourBasedOnNote(blockNote);

        var materialTemp = colourBlockRenderer.materials;
        materialTemp[materialInArray] = colourMaterial;
        colourBlockRenderer.materials = materialTemp;

        var material2Temp = colourQuestionRenderer.materials;
        //material2Temp[0].EnableKeyword("_EMISSION");
        //material2Temp[0].color = colourMaterial.GetColor("_EmissionColor");
        material2Temp[0] = colourMaterial;
        colourQuestionRenderer.materials = material2Temp;

        var material3Temp = colourNoteRenderer.materials;
        // material3Temp[0].EnableKeyword("_EMISSION");
        //material3Temp[0].color = colourMaterial.GetColor("_EmissionColor");
        material3Temp[0] = colourMaterial;
        colourNoteRenderer.materials = material3Temp;
    }
}

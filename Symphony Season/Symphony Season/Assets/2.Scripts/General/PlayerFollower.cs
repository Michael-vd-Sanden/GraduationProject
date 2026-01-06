using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [Header("-------------- Required Objects")]
    [SerializeField] private GameObject PlayerObject;
    [SerializeField] private Material playerMaterial;

    [Header("-------------- Changeble Values")]
    [SerializeField] private float Yoffset;
    [SerializeField] private float Xoffset;
    [SerializeField] private float Zoffset;

    [Header("-------------- Background Values (do not change)")]
    [SerializeField] private PlayerMouseMovement playerMovement;

    private void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMouseMovement>();
        ToggleLeft(0f);
        ToggleMoving(0f);
        ToggleHolding(0f);
    }

    void Update()
    {
        transform.position = new Vector3(PlayerObject.transform.position.x + Xoffset, PlayerObject.transform.position.y + Yoffset, PlayerObject.transform.position.z + Zoffset);
        if(playerMovement.isMoving) 
        { 
            ToggleMoving(1f); 
            if(playerMovement.isMovingLeft) { ToggleLeft(1f); }
            else if (!playerMovement.isMovingLeft) { ToggleLeft(0f); }
        }
        else if(!playerMovement.isMoving) { ToggleMoving(0f); }


    }

    public void ToggleLeft(float toggle)
    {
        playerMaterial.SetFloat("_IsLeft", toggle);
    }

    public void ToggleMoving(float toggle)
    {
        playerMaterial.SetFloat("_IsMoving", toggle);
    }

    public void ToggleHolding(float toggle)
    {
        playerMaterial.SetFloat("_IsHolding", toggle);
    }
}

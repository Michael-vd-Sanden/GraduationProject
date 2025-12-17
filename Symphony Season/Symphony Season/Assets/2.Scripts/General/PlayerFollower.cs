using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    [SerializeField] private GameObject PlayerObject;
    [SerializeField] private float Yoffset;

    [SerializeField] private PlayerMouseMovement playerMovement;

    [SerializeField] private Material playerMaterial;

    private void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMouseMovement>();
    }

    void Update()
    {
        transform.position = new Vector3(PlayerObject.transform.position.x, PlayerObject.transform.position.y + Yoffset, PlayerObject.transform.position.z);
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

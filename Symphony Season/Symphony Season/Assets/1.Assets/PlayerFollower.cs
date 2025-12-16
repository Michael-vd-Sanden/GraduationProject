using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public GameObject PlayerObject;
    public float Yoffset;
    void Update()
    {
        transform.position = PlayerObject.transform.position;
        transform.position = new Vector3(PlayerObject.transform.position.x, PlayerObject.transform.position.y + Yoffset, PlayerObject.transform.position.z);
    }
}

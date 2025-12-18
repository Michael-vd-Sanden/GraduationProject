using UnityEngine;

public class PopupRotator : MonoBehaviour
{
    private GameObject GlobalRoot;

    private void Awake()
    {
        GlobalRoot = GameObject.FindGameObjectWithTag("GlobalRoot");

        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            GlobalRoot.transform.eulerAngles.y - 45,
            transform.eulerAngles.z
            );
    }
}

using UnityEngine;

public class QuestionAndAnswerBehaviour : MonoBehaviour
{
    public GameObject GlobalRoot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x, 
            GlobalRoot.transform.eulerAngles.y - 45, 
            transform.eulerAngles.z
            );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

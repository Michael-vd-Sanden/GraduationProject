using UnityEngine;

public class MazeTriggers : MonoBehaviour
{
    private MazePuzzle mazePuzzle;

    private void Awake()
    {
        mazePuzzle = FindFirstObjectByType<MazePuzzle>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            switch (this.gameObject.tag)
            {
                case "End":
                    mazePuzzle.EndMaze();
                    break;
                case "":
                    break;
            }
        }
    }
}

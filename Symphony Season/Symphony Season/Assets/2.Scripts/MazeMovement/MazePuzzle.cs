using UnityEngine;

public class MazePuzzle : MonoBehaviour
{
    [SerializeField] private int questionNumber;

    public void StartQuestion()
    {
        Debug.Log("Started question");
        questionNumber = Random.Range(0, 11);
        //iets met een list met alle mogelijke vragen, waar die dan de [i] van pakt met de range
    }
    
    public void EndMaze()
    {
        Debug.Log("End maze");
    }
}

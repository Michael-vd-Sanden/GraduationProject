using UnityEngine;

public class LevelIndex : MonoBehaviour
{
    public int FloorIndex = 0;
        //This variable will determine which level is currently selectable.
        //Selectable levels have open dioramas, unselectable levels have closed walls.
        //By reading this number, animators for each level will know if they are closed or open.
        //Will be changed by the up / down buttons in the UI.

    //public float SelectionDelay;
        //SelectionDelay will make sure levels can't be selected before the animation is fully (or almost) complete.

    //public void Awake()
    //{
        //to do: Add code to make sure whenever you exit a level, the tower starts at that floor
        //should probably fix something in the animator to do that too
        //maybe use an integer in the animator to change the starting Idle position when the player returns to the level select.
    //}

    public void NextLevel()
    {
        FloorIndex += 1;
    }

    public void PreviousLevel()
    {
        FloorIndex -= 1;
    }
}

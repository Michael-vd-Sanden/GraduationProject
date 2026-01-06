using UnityEngine;

public class TitleScreenCameraMove : MonoBehaviour
{
    public Animator TowerAnimator;

    public void MoveUp()
    {
        TowerAnimator.SetFloat("Speed", 1);
    }
    public void MoveDown()
    {
        TowerAnimator.SetFloat("Speed", -1);
    }
}

using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.Rendering;
using static UnityEditor.FilePathAttribute;
using static UnityEngine.Rendering.DebugUI.Table;

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

using UnityEngine;

public class TriggerSetter : MonoBehaviour
{
    public Animator[] Animators;
    public string TriggerToSet;
    public bool PlayOnAwake;

    public void Awake()
    {
        if (PlayOnAwake)
        {
            SetTrigger();
        }
    }
    public void SetTrigger()
    {
        for (int i = 0; i < Animators.Length; i++)
        {
            Animators[i].SetTrigger(TriggerToSet);
        }
    }
}

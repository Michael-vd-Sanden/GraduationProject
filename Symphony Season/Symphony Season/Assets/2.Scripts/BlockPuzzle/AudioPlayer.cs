using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    [Header("-------------- Required Objects")]
    [SerializeField] private AudioClip[] effectClips;
    [SerializeField] private AudioClip[] ambientClips;
    public AudioSource musicSource, effectSource;

    private int activeMusicClip = 0;
    private int activeEffectClip = 0; 

    private void Update()
    {
        if(!musicSource.isPlaying) 
        { PlayMusic(); }
    }

    public void PlayMusic()
    {
        if (activeMusicClip < ambientClips.Length)
        {
            musicSource.clip = ambientClips[activeMusicClip];
            activeMusicClip++;
        }
        else
        {
            activeMusicClip = 0;
            musicSource.clip = ambientClips[activeMusicClip];
        }
        musicSource.Play();
    }

    public void PlayEffect(string effectName)
    {
        switch (effectName) 
        {
            case "A":
                activeEffectClip = 0;
                break;
            case "B":
                activeEffectClip = 1;
                break;
            case "C":
                activeEffectClip = 2;
                break;
            case "D":
                activeEffectClip = 3;
                break;
            case "E":
                activeEffectClip = 4;
                break;
            case "F":
                activeEffectClip = 5;
                break;
            case "G":
                activeEffectClip = 6;
                break;
            case "Wrong":
                activeEffectClip = 7;
                break;
        }
        effectSource.clip = effectClips[activeEffectClip];
        effectSource.Play();
    }

}

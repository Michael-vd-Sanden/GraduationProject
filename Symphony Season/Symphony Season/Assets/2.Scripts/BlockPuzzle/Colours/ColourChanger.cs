using System.Collections.Generic;
using UnityEngine;

public class ColourChanger : MonoBehaviour
{
    [Header("-------------- Required Objects")]
    public List<Material> colours;

    public Material ChangeColourBasedOnNote(string note)
    {
        Material m;
        switch (note)
        {
            case "Ab":
                m = colours[0];
                break;
            case "A":
                m = colours[0];
                break;
            case "A#":
                m = colours[0];
                break;
            case "Bb":
                m = colours[1];
                break;
            case "B":
                m = colours[1];
                break;
            case "C":
                m = colours[2];
                break;
            case "C#":
                m = colours[2];
                break;
            case "Db":
                m = colours[3];
                break;
            case "D":
                m = colours[3];
                break;
            case "D#":
                m = colours[3];
                break;
            case "Eb":
                m = colours[4];
                break;
            case "E":
                m = colours[4];
                break;
            case "F":
                m = colours[5];
                break;
            case "F#":
                m = colours[5];
                break;
            case "Gb":
                m = colours[6];
                break;
            case "G":
                m = colours[6];
                break;
            case "G#":
                m = colours[6];
                break;
            default:
                m = colours[0];
                break;
        }
        return m;
    }
}

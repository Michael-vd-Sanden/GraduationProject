using System.Collections.Generic;
using UnityEngine;

public class ColourChanger : MonoBehaviour
{
    public List<Material> colours;

    public Material ChangeColourBasedOnNote(string note)
    {
        Material m;
        switch (note)
        {
            case "A":
                m = colours[0];
                break;
            case "B":
                m = colours[1];
                break;
            case "C":
                m = colours[2];
                break;
            case "D":
                m = colours[3];
                break;
            case "E":
                m = colours[4];
                break;
            case "F":
                m = colours[5];
                break;
            case "G":
                m = colours[6];
                break;
            default:
                m = colours[0];
                break;
        }
        return m;
    }
}

using UnityEditor.SpeedTree.Importer;
using UnityEngine;

public class ColourChange: MonoBehaviour
{
    public Renderer ColourChangeObject;
    private Material ThisMaterial;
    private Material[] ColourChangeMat;
    public int MaterialIndex = 4;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ThisMaterial = GetComponent<Material>();
        ColourChangeMat = ColourChangeObject.materials;
        ThisMaterial.SetColor("_Colour", ColourChangeMat[MaterialIndex].color);
    }
}

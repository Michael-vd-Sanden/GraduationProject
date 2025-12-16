using UnityEngine;

public class NoteColourChanger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private ColourChanger colourChanger;

    public string note;

    private void Awake()
    {
        spriteRenderer= GetComponent<SpriteRenderer>();
        colourChanger = FindFirstObjectByType<ColourChanger>();
        spriteRenderer.material = colourChanger.ChangeColourBasedOnNote(note);
    }
}

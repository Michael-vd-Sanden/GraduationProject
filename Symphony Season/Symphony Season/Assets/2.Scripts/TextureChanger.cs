using UnityEngine;

public class TextureChanger : MonoBehaviour
{
    public bool ChangeBackMat = false;
    public bool HardMode = false;
    public Material FrontMat;
    public Material BackMat;
    public Texture2D[] LevelScreenshotsFront;
    public Texture2D[] LevelScreenshotsHardMode;
    public Texture2D[] NextOne;
    public LevelIndex LevelIndexer;
    public void TextureChange()
    {
        if (ChangeBackMat && !HardMode)
        {
            NextTexture(BackMat, LevelScreenshotsFront[LevelIndexer.FloorIndex]);
            ChangeBackMat = false;
        }
        else if (!ChangeBackMat && !HardMode)
        {
            NextTexture(FrontMat, LevelScreenshotsFront[LevelIndexer.FloorIndex]);
            ChangeBackMat = true;
        }
        else if (ChangeBackMat && HardMode)
        {
            NextTexture(BackMat, LevelScreenshotsHardMode[LevelIndexer.FloorIndex]);
            ChangeBackMat = false;
        }
        else if (!ChangeBackMat && HardMode)
        {
            NextTexture(FrontMat, LevelScreenshotsHardMode[LevelIndexer.FloorIndex]);
            ChangeBackMat = true;
        }

    }

    public void HardModeShift()
    {
        if (!HardMode) { HardMode = true; }
        else { HardMode = false; }
    }
    private void NextTexture(Material MatToUse, Texture2D TextureToUse)
    {
        MatToUse.mainTexture = TextureToUse;
    }

}

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
    public void TextureChange(int LevelIndex)
    {
        if (ChangeBackMat)
        {
            NextTexture(BackMat, LevelScreenshotsFront[LevelIndex]);
            ChangeBackMat = false;
        }
        else if (!ChangeBackMat)
        {
            NextTexture(FrontMat, LevelScreenshotsFront[LevelIndex]);
            ChangeBackMat = true;
        }
        else if (ChangeBackMat && HardMode)
        {
            NextTexture(BackMat, LevelScreenshotsHardMode[LevelIndex]);
            ChangeBackMat = false;
        }
        else if (!ChangeBackMat && HardMode)
        {
            NextTexture(FrontMat, LevelScreenshotsHardMode[LevelIndex]);
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

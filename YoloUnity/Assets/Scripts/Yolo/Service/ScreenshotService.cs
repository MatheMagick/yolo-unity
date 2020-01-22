using UnityEngine;
using System.IO;

public class HiResScreenShots 
{
    public byte[] GetScreenShot(Camera camera, Vector2Int size) 
    {
        int resWidth = size.x;
        int resHeight = size.y;
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        // TODO Destroy
        //Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();

        //SaveScreenshotAsFile(resWidth, resHeight, bytes);

        return bytes;
    }

    private static void SaveScreenshotAsFile(int resWidth, int resHeight, byte[] bytes)
    {
        string filename = ScreenShotName(resWidth, resHeight);
        Directory.CreateDirectory(Path.GetDirectoryName(filename));
        System.IO.File.WriteAllBytes(filename, bytes);

        Debug.Log(string.Format("Took screenshot to: {0}", filename));
    }

    private static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png", 
        Application.dataPath, 
        width, height, 
        System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }
}
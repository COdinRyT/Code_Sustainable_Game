using System;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Screenshot : MonoBehaviour
{
    private Camera cam;

    [SerializeField] private string pathFolder = "Screenshots"; // Default folder
    [SerializeField] private string prefix = "Screenshot"; // Default filename prefix

    private void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam == null)
        {
            Debug.LogError("No Camera found on this GameObject. Screenshot script requires a Camera.");
        }
    }

    // Adds a custom context menu option in the Inspector
    [ContextMenu("Take Screenshot")]
    public void TakeScreenshot()
    {
        if (cam == null)
        {
            cam = GetComponent<Camera>();
        }

        if (!Directory.Exists(pathFolder))
        {
            Directory.CreateDirectory(pathFolder);
        }

        string fileName = $"{prefix}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
        string fullPath = Path.Combine(pathFolder, fileName);

        RenderTexture rt = new RenderTexture(256, 256, 24);
        cam.targetTexture = rt;

        Texture2D screenShot = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        cam.Render();

        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, 256, 256), 0, 0);
        screenShot.Apply();

        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(fullPath, bytes);

#if UNITY_EDITOR
        AssetDatabase.Refresh(); // Refresh the editor to see the new screenshot
#endif

        Debug.Log($"Screenshot saved to: {fullPath}");
    }
}

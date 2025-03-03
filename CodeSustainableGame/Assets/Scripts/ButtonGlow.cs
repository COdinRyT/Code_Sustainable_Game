using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GlowAndSparkle : MonoBehaviour
{
    public Image glow;    
    public Image sparkle;  
    [Range(0, 255)] public int transparency = 255;

    public static GlowAndSparkle Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        float alpha = transparency / 255f; 

        if (glow != null) glow.color = new Color(glow.color.r, glow.color.g, glow.color.b, alpha);
        if (sparkle != null) sparkle.color = new Color(sparkle.color.r, sparkle.color.g, sparkle.color.b, alpha);
    }
}

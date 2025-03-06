using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkCanvasToCamera : MonoBehaviour
{
    public Camera camera;
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        canvas.GetComponent<Canvas>().worldCamera = camera;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public GameObject tile;
    [SerializeField] private Material tileMaterial;

    // Start is called before the first frame update
    void Start()
    {
        tile = GetComponent<GameObject>();
        tileMaterial = GetComponent<Material>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        int r = 0;
        int g = 0;  
        int b = 255;
        int alphaValue = 255;

        tileMaterial.color = new Color(r, g, b, alphaValue);
    }
}

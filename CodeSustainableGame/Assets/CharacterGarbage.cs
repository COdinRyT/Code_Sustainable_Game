using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterGarbage : MonoBehaviour
{
    public GameObject slider;
    public int garbageHolding = 0;
    public int maxGarbageHolding = 100;
    // Start is called before the first frame update
    void Start()
    {
        maxGarbageHolding = 100;
    }

    // Update is called once per frame
    void Update()
    {
        slider.GetComponent<Slider>().value = garbageHolding;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class CheckIfOnGarbage : MonoBehaviour
{
    public GameObject GarbageStorage;
    public GameObject Player;
    public Transform PlayerTransform;
    public float x;
    public float y;
    public float z;
    private Vector3 madeUpVector3;
    public static CheckIfOnGarbage Instance { get; private set; }
    public GameObject[] allChildren;

    public bool PlayerAndGarbageCollision = false;

    public GameObject ProgressBar;
    public GameObject WorldCanvas;

    void Start()
    {
        GarbageStorage = GameObject.Find("GarbageStorage");
    }
    public void CheckCollisionBetweenPlayerAndGarbage()
    {
        madeUpVector3 = new Vector3(gameObject.transform.position.x,y,gameObject.transform.position.z);
        GetChildren();
        for (int i = 0; i < allChildren.Length; i++)
        {
            //Debug.Log("Garbage:" + allChildren[i].transform.position);
            //Debug.Log("Player:" + Player.transform.position);
            /*
            if (allChildren[i].transform.position == madeUpVector3)
            {
                Debug.Log("At same spot. We have collision");
            }
            */
            //Debug.Log("Garbage: " + allChildren[i].transform.position);
            //Debug.Log("Player x: " + madeUpVector3.x + " z: "+ madeUpVector3.z);
            if (allChildren[i].transform.position.x == madeUpVector3.x && allChildren[i].transform.position.z == madeUpVector3.z)
            {
                Vector3 currentScale = allChildren[i].transform.localScale;
                allChildren[i].GetComponent<Garbage>().currentHealth -= 25;
                allChildren[i].transform.localScale = currentScale * 0.8f;
                Debug.Log(allChildren[i].GetComponent<Garbage>().currentHealth);
                //Destroy(allChildren[i]);
                Debug.Log("Same spot, We have collision.");
                PlayerAndGarbageCollision = true;
            }
            //child is your child transform
        }

    }

    // Start is called before the first frame update
    private void GetChildren()
    {
        allChildren = new GameObject[GarbageStorage.transform.childCount];
        for (int i = 0; i < allChildren.Length; i++)
        {
            allChildren[i] = GarbageStorage.transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}

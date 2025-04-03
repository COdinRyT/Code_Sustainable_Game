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
            if (allChildren[i].transform.position.x == madeUpVector3.x && allChildren[i].transform.position.z == madeUpVector3.z)
            {
                Vector3 currentScale = allChildren[i].transform.localScale;
                if (allChildren[i].GetComponent<Garbage>())
                {
                    if (allChildren[i].GetComponent<Garbage>().currentHealth > 0)
                    {
                        allChildren[i].GetComponent<Garbage>().currentHealth -= 25;
                        allChildren[i].transform.localScale = currentScale * 0.8f;
                        Debug.Log(allChildren[i].GetComponent<Garbage>().currentHealth);
                        Debug.Log("Same spot, We have collision.");
                        PlayerAndGarbageCollision = true;
                    }
                }
            }
        }

    }
    private void GetChildren()
    {
        allChildren = new GameObject[GarbageStorage.transform.childCount];
        for (int i = 0; i < allChildren.Length; i++)
        {
            allChildren[i] = GarbageStorage.transform.GetChild(i).gameObject;
        }
    }
}

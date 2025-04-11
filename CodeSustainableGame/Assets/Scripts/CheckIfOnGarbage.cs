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
        //Debug.Log("Checking collision!");

        // Snap the player's x and z position to the nearest whole unit (grid snap)
        madeUpVector3 = new Vector3(
            Mathf.Round(gameObject.transform.position.x),  // Round X to nearest whole number
            y,  // Keep the Y position as is
            Mathf.Round(gameObject.transform.position.z)   // Round Z to nearest whole number
        );

        GetChildren();  // Populate the array of all children (garbage items)

        for (int i = 0; i < allChildren.Length; i++)
        {
            // Debugging to ensure the positions are being checked correctly
            //Debug.Log("MadeUpVector (Rounded): " + madeUpVector3);
            //Debug.Log("Garbage pos: " + allChildren[i].transform.position);

            // Compare the rounded player position with the garbage's position
            if (Mathf.Approximately(allChildren[i].transform.position.x, madeUpVector3.x) &&
                Mathf.Approximately(allChildren[i].transform.position.z, madeUpVector3.z))
            {
                //Debug.Log("X and Z are the same");

                Vector3 currentScale = allChildren[i].transform.localScale;

                if (allChildren[i].GetComponent<Garbage>())
                {
                    if (allChildren[i].GetComponent<Garbage>().currentHealth > 0)
                    {
                        Player.GetComponent<CharacterGarbage>().garbageHolding += 25;
                        allChildren[i].GetComponent<Garbage>().currentHealth -= 25;
                        allChildren[i].transform.localScale = currentScale * 0.8f;
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

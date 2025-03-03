using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private Camera camera;

    UpdateUI updateUI;

    public Queue<GameObject> characters = new Queue<GameObject>(); //Character queue
    public int maxVolunteers = 5;

    private void Awake()
    {
        Instance = this;         
    }
     
    public int currentTurn;
    public int startTurn;
    public int maxTurn;
    public int currentPeople;
    public int maxPeople;
    public int currentMoney;
    public bool endTurn;

    public GameObject TerrainGroup;
    public GameObject Garbage;
    public List<GameObject> tag_targets = new List<GameObject>();

    public Transform parentTransform;

    private float chanceOfGarbage = 9;
    private float randomNumber;
    private float currentx;
    private float currenty;
    private Vector3 spot;

    NewBehaviourScript pointClickMovement; 


    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        currentTurn = startTurn;
        SpawnGarbage();
        pointClickMovement = FindAnyObjectByType<NewBehaviourScript>();
        updateUI = FindAnyObjectByType<UpdateUI>();
        if(updateUI == null)
        {
            Debug.Log("UI manager is not assigned to game manager!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("End turn: " + endTurn);
        //Debug.Log(" Current Turn: " + currentTurn);
        //Debug.Log("Max turn: " + maxTurn);


        if (endTurn && currentTurn < maxTurn)
        {
            endTurn = false;
            currentTurn++;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject selectedObject = hit.collider.gameObject;

                if(selectedObject.layer == 6)
                {
                    GameObject rootObject = selectedObject.transform.root.gameObject;
                    ConfirmVolunteer(rootObject);
                }
            }
        }
    }

    //Add character object into queue when function is called
    public void ConfirmVolunteer(GameObject character)
    {
        if (!characters.Contains(character))
        {
            characters.Enqueue(character);
            updateUI.UpdateQueueUI(new List<GameObject>(characters));
        }
    }

    public void RunNextVolunteer()
    {
        if(characters.Count == 0)
        {
            Debug.Log("Volunteers occupied");
            return;
        }

      GameObject currentCharacter = characters.Dequeue();
       
        if(pointClickMovement.isTileSelected)
        {
           
        }
    }

    //public void ConfirmTask(GameObject character)
    //{
    //    if (!characters.Contains(character))
    //    {
    //        characters.Enqueue(character);
    //        Debug.Log(character.name + "added to queue");
    //    }        
    //}

    //public void RunNextCharacter()
    //{
    //    if(characters.Count == 0)
    //    {
    //        Debug.Log("All volunteers occupied");
    //        return;
    //    }

    //    GameObject currentCharacter = characters.Dequeue();
    //    if(pointClickMovement.isTileSelected)
    //    {
    //        Vector3 tilePos = pointClickMovement.selectedTile.transform.position;
    //        pointClickMovement.MovePlayer(tilePos);
    //        if (characters.Count > 0)
    //        {
    //            Debug.Log("Next volunteer in the queue: " + characters.Peek().name);
    //        }
    //        else
    //        {
    //            Debug.Log("All volunteers are assigned tasks.");
    //        }
    //    }

       

    //}

    private void SpawnGarbage()
    {
        foreach (Transform child in TerrainGroup.transform)
        {
            GameObject obj = child.gameObject;
            //Debug.Log(obj.layer);
            if (obj.layer == 7)
            {
                randomNumber = Random.Range(0, 10);
                //Debug.Log(randomNumber);
                if (randomNumber >= chanceOfGarbage)
                {
                    //Debug.Log("what");
                    spot = new Vector3(obj.transform.position.x, .51f, obj.transform.position.z);
                    Instantiate(Garbage, spot, Quaternion.identity, parentTransform);
                    // Do things with obj
                }
            }
        }
    }

    public void OnClick()
    {
       if(pointClickMovement.isTileSelected && characters.Count > 0)
        {
            RunNextVolunteer();
        } 
       else if(characters.Count == 0)
        {
            Debug.Log("No volunteers available");
        }
    }

    public void WebsiteLink() //This is to link the Pollution Probe website 
    {
        Application.OpenURL("https://www.pollutionprobe.org"); //When a player selects a button, this function will be called
    }
}

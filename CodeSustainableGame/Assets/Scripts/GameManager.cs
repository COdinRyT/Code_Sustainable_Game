using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private Camera camera;

    UpdateUI updateUI;

    public Queue<GameObject> characters = new Queue<GameObject>(); //Character queue
    public Queue<GameObject> tileQueue = new Queue<GameObject>(); //Queue for tile selection
    public GameObject prefab;

    private void Awake()
    {
        Instance = this;
        updateUI = FindAnyObjectByType<UpdateUI>();
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
    private int tileLimit;


    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        currentTurn = startTurn;
        SpawnGarbage();
        pointClickMovement = FindAnyObjectByType<NewBehaviourScript>();        
        updateUI = FindAnyObjectByType<UpdateUI>();
        updateUI.UpdateQueueUI(new List<GameObject>(characters));
        if (updateUI == null)
        {
            Debug.Log("UI manager is not assigned to game manager!");
        }

        tileLimit = characters.Count;

        FirstPlayer();
    }

    public void FirstPlayer() {
        GameObject currentCharacter = FindAnyObjectByType<NewBehaviourScript>().gameObject;
        pointClickMovement.SelectPlayer(currentCharacter);  

        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("End turn: " + endTurn);
        //Debug.Log(" Current Turn: " + currentTurn);
        //Debug.Log("Max turn: " + maxTurn);
        tileLimit = characters.Count;

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
                if(selectedObject.layer == 7 && characters.Count == 2)
                {
                    ConfirmTilePlacement(hit.collider.gameObject);
                }
            }
        }
    }

    //Add character object into queue when function is called
    public void ConfirmVolunteer(GameObject character)
    {
        

        if (!characters.Contains(character))
        {
            Debug.Log("Confirming volunteer");
            characters.Enqueue(character);
            //pointClickMovement.SelectPlayer(character);           
        }
    }
    /*
    public void ConfirmTilePlacement(GameObject tile)
    {        
        if(tileQueue.Count < tileLimit)
        {            
            pointClickMovement.SelectTile(tile, prefab);
            Debug.Log($"Tiles selectable: {characters.Count}");
            tileQueue.Enqueue(tile);
        }
        else
        {
            Debug.Log("No more paths to select");
        }
    }
    */
    public void ConfirmTilePlacement(GameObject tile)
    {
        if (tileQueue.Count < tileLimit)
        {
            pointClickMovement.SelectTile(tile, prefab);
            Debug.Log($"Tiles selectable: {characters.Count}");
            tileQueue.Enqueue(tile);
        }
        else
        {
            Debug.Log("No more paths to select");
        }
    }

    public void DoTask()
    {
        if (characters.Count != tileQueue.Count)
        {
            Debug.LogError("Mismatch between character and tile queue sizes!");
            return;
        }

        StartCoroutine(MoveCharacterSequence());        
    }

    private IEnumerator MoveCharacterSequence()
    {
        while(characters.Count > 0 && tileQueue.Count > 0)
        {
            GameObject currentCharacter = characters.Dequeue();
            GameObject targetTile = tileQueue.Dequeue();
            updateUI.UpdateQueueUI(new List<GameObject>(characters));

            yield return StartCoroutine(pointClickMovement.MovePlayer());
        }
    }

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

    

    public void WebsiteLink() //This is to link the Pollution Probe website 
    {
        Application.OpenURL("https://www.pollutionprobe.org"); //When a player selects a button, this function will be called
    }
}

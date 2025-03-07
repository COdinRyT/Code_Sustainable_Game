using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CleanGarbage : MonoBehaviour
{
    [Header("Progress Bar Settings/Variables")]
    private Image progressBar;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    [Header("Trash Collection Settings/Variables")]
    GameManager gameManager;
    public GameObject trash;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager>();
        progressBar = GetComponentInChildren<Image>();
        progressBar.gameObject.SetActive(false);

        trash = gameManager.Garbage;
    }

    // Update is called once per frame
    void Update()
    {
        TrashDetection();
        progressBar.transform.rotation = camera.transform.rotation;
        progressBar.transform.position = target.position + offset;
    }

    public void TrashDetection()
    {
       
    }
}

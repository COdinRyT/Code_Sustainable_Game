using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurn : MonoBehaviour
{
    public GameObject advertisementPanel; 

    public void OnClick()
    {
        if (GameManager.Instance.endTurn == false)
        {
            GameManager.Instance.endTurn = true;

            advertisementPanel.SetActive(true);
        }
    }
}

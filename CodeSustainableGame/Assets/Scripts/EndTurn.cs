using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EndTurn : MonoBehaviour
{

    public void OnClick()
    {
        //Shop.Instance.truckSpawns = true;

        Debug.Log("CLicked");
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is null");
            return;
        }

        if (GameManager.Instance.endTurn == false)
        {
            GameManager.Instance.endTurn = true;
            Debug.Log("End turn end turn");
        }
    }
}
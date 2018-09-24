using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public MapManager mapManager;

    [HideInInspector] public int pos;
    //[HideInInspector] public bool playersTurn;
    [HideInInspector] public int moveAttempt;
    [HideInInspector] public int turnsToWait;

    private void Awake()
    {
        mapManager = GameObject.Find("GameManager").GetComponent<MapManager>();
        pos = 0;
        //playersTurn = false;
        moveAttempt = 0;
        turnsToWait = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveAttempt = pos + mapManager.labSize;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveAttempt = pos - mapManager.labSize;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveAttempt = pos + 1;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveAttempt = pos - 1;
        }
    }
}

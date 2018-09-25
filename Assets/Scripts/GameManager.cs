using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public float turnsDelay = 0.5f;
    private MapManager mapManager;

    private void Awake()
    {
        //Get MapManager
        mapManager = GetComponent<MapManager>();
    }

    void Start()
    {
        InitGame();
    }

    void InitGame()
    {
        //Initialize random map
        mapManager.InitializeRandomMap();
        mapManager.GraphFromMap();
        StartCoroutine("NewTurn");

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StopCoroutine("NewTurn");
            mapManager.CleanMap();
            InitGame();
        }
        else if(Input.GetKeyDown(KeyCode.G))
        {
            mapManager.GameOver();
        }
        else if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    IEnumerator NewTurn()
    {
        while(!mapManager.isGameOver)
        {
            mapManager.NewTurn();
            yield return new WaitForSeconds(turnsDelay);
        } 
    }
}

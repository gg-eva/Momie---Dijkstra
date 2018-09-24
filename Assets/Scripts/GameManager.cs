using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            mapManager.CleanMap();
            InitGame();
        }
    }
}

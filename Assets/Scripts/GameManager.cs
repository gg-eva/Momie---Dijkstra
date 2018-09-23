using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //public float levelStartDelay = 2f;                      //Time to wait before starting level, in seconds.
    //public float turnDelay = 0.1f;                          //Delay between each Player turn.
    //public int playerFoodPoints = 100;                      //Starting value for Player food points.
    //public static GameManager instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.
    //[HideInInspector] public bool playersTurn = true;       //Boolean to check if it's players turn, hidden in inspector but public.

    //Enemy enemy;
    //private bool enemyMoving;                             //Boolean to check if enemies are moving.
    

    private MapManager mapManager;
    private bool doingSetup = true;


    void Start()
    {
        //Get MapManager
        mapManager = GetComponent<MapManager>();
        //while (mapManager.doingSetup);
        InitGame();
    }

    void InitGame()
    {
        doingSetup = true;
        //Initialize random map
        //mapManager.InitializeRandomMap();

        //TOOD
        //mapManager.GraphFromMap();
    }

    //void Update()
    //{

    //    if (playersTurn || enemyMoving || doingSetup)
    //        return;

    //    //Start moving enemies.
    //    StartCoroutine(MoveEnemy());
    //}

    ////Call this to add the passed in Enemy to the List of Enemy objects.
    //public void AddEnemyToList(Enemy script)
    //{
    //    //Add Enemy to List enemies.
    //    enemy.Add(script);
    //}


    ////GameOver is called when the player reaches 0 food points
    //public void GameOver()
    //{
    //    //Set levelText to display number of levels passed and game over message
    //    levelText.text = "After " + level + " days, you starved.";

    //    //Enable black background image gameObject.
    //    levelImage.SetActive(true);

    //    //Disable this GameManager.
    //    enabled = false;
    //}


    //IEnumerator MoveEnemy()
    //{
    //    //So player is unable to move.
    //    enemyMoving = true;

    //    //Wait for turnDelay seconds, defaults to .1 (100 ms).
    //    yield return new WaitForSeconds(turnDelay);

    //    enemy.MoveEnemy();
    //    yield return new WaitForSeconds(enemy.moveTime);

    //    //playersTurn to true so player can move.
    //    playersTurn = true;

    //    //Enemy is done moving
    //    enemyMoving = false;
    //}
}

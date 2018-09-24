using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public MapManager mapManager;

    [HideInInspector] public int pos;

    private void Start()
    {
        mapManager = GameObject.Find("GameManager").GetComponent<MapManager>();
        pos = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

        }
    }
}

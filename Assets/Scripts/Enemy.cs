using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public MapManager mapManager;

    [HideInInspector] public int pos;

    private void Awake()
    {
        mapManager = GameObject.Find("GameManager").GetComponent<MapManager>();
        pos = mapManager.labSize * mapManager.labSize - 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dijkstra;

public class MapManager : MonoBehaviour
{

    public int labSize = 5;     //Number of tiles of one side (square)
    public Trace traceRef;         //Trace the shortest path
    public Tile[] tilesPrefab;  //Each possible tile from assets
    public Skull[] skulls; //Getting skull sprites for difficulty of paths
    public Player playerRef;   //Player
    public Enemy enemyRef;     //Enemy

    //Sub list to make a better labyrinth
    List<Tile> tilesBorderUp = new List<Tile>();
    List<Tile> tilesBorderRight = new List<Tile>();
    List<Tile> tilesBorderLeft = new List<Tile>();
    List<Tile> tilesBorderDown = new List<Tile>();

    Tile tileCornerUpRight;
    Tile tileCornerUpLeft;
    Tile tileCornerDownRight;
    Tile tileCornerDownLeft;

    //Size info
    float tileScale;
    float tileSize;
    float skullScale;
    float playerScale;
    float enemyScale;

    //Instantiated tile map
    List<Tile> labyrinthTiles = new List<Tile>();
    List<Skull> labyrinthSkulls = new List<Skull>();

    //Instantiate
    Player player;
    Enemy enemy;
    Trace trace;

    //For gameManagement
    [HideInInspector] public bool doingSetup = true;

    //Before any start
    private void Awake()
    {
        //Initialize useful variables
        tileScale = tilesPrefab[0].GetComponent<Transform>().localScale.x / labSize;
        tileSize = tilesPrefab[0].GetComponent<SpriteRenderer>().bounds.size.x / labSize;
        InitializeTilesSubLists();

        skullScale = skulls[0].GetComponent<Transform>().localScale.x / labSize;
        playerScale = playerRef.GetComponent<Transform>().localScale.x / labSize;
        enemyScale = enemyRef.GetComponent<Transform>().localScale.x / labSize;
    }

    // Use this for initialization
    void Start()
    {
        doingSetup = false;
    }

    void InitializeTilesSubLists()
    {
        //Files sublist to close the labyrinth
        foreach (Tile tile in tilesPrefab)
        {
            if (!tile.up)
            {
                tilesBorderUp.Add(tile);
                if (!tile.left)
                    tileCornerUpLeft = tile;
                if (!tile.right)
                    tileCornerUpRight = tile;
            }
            if (!tile.down)
            {
                tilesBorderDown.Add(tile);
                if (!tile.left)
                    tileCornerDownLeft = tile;
                if (!tile.right)
                    tileCornerDownRight = tile;
            }
            if (!tile.left)
                tilesBorderLeft.Add(tile);
            if (!tile.right)
                tilesBorderRight.Add(tile);
        }
    }

    public void CleanMap()
    {

        //Instantiated tile map
        //Destroy objects
        foreach (Tile tile in labyrinthTiles)
            Destroy(tile.gameObject);

        foreach (Skull skull in labyrinthSkulls)
            Destroy(skull.gameObject);

        //Empty list
        labyrinthTiles.Clear();
        labyrinthSkulls.Clear();

        //Instantiated player, enemy and trace
        Destroy(player.gameObject);
        Destroy(enemy.gameObject);
        Destroy(trace.gameObject);
    }

    //Initialize a random map
    public void InitializeRandomMap()
    {
        //Choosing each tile type for each place in the map
        for (int y = 0; y < labSize; ++y)
        {
            for (int x = 0; x < labSize; ++x)
            {
                Tile tileRef;

                if (x == 0)
                {
                    if (y == 0)
                        tileRef = tileCornerDownLeft;
                    else if (y == labSize - 1)
                        tileRef = tileCornerUpLeft;
                    else
                        tileRef = FindRandomTileInList(tilesBorderLeft);
                }
                else if (x == labSize - 1)
                {
                    if (y == 0)
                        tileRef = tileCornerDownRight;
                    else if (y == labSize - 1)
                        tileRef = tileCornerUpRight;
                    else
                        tileRef = FindRandomTileInList(tilesBorderRight);
                }
                else if (y == 0)
                    tileRef = FindRandomTileInList(tilesBorderDown);
                else if (y == labSize - 1)
                    tileRef = FindRandomTileInList(tilesBorderUp);
                else
                    tileRef = FindRandomTile();

                //Adding random difficulty to tile
                Skull skullRef = skulls[Random.Range(0,skulls.Length)]; 

                //Instantiating and resizing
                //Tile
                Vector3 pos = GetTilePosFromIndex(GetTileIndexFromCoordinates(new Vector2Int(x, y)));
                Tile tile = Instantiate(tileRef, pos, Quaternion.identity);
                tile.GetComponent<Transform>().localScale = new Vector3(tileScale, tileScale);

                //Skulls
                Skull skull = Instantiate(skullRef, pos, Quaternion.identity);
                skull.GetComponent<Transform>().localScale = new Vector3(skullScale, skullScale);

                //Keeping tile map info
                labyrinthTiles.Add(tile);
                labyrinthSkulls.Add(skull);
            }
        }


        //Player
        Vector3 pos2 = GetTilePosFromIndex(0);
        player = Instantiate(playerRef, pos2, Quaternion.identity);
        player.GetComponent<Transform>().localScale = new Vector3(playerScale, playerScale);

        //Enemy
        pos2 = GetTilePosFromIndex(labSize*labSize - 1);
        enemy = Instantiate(enemyRef, pos2, Quaternion.identity);
        enemy.GetComponent<Transform>().localScale = new Vector3(enemyScale, enemyScale);

        //Trace
        trace = Instantiate(traceRef);
    }



    public void GraphFromMap()
    {
        List<Node> nodes = new List<Node>();
        List<Edge> edges = new List<Edge>();
        for (int y = 0; y < labSize; ++y)
        {
            for (int x = 0; x < labSize; ++x)
            {

                int currentPos = GetTileIndexFromCoordinates(new Vector2Int(x, y));
                nodes.Add(new Node(currentPos.ToString()));
                Tile currentTile = labyrinthTiles[currentPos];

                if (x > 0)
                {
                    int leftPos = currentPos - 1;
                    Tile leftTile = labyrinthTiles[leftPos];
                    if (currentTile.left && leftTile.right)
                    {
                        //Directed graph, bilateral with 2 differents difficulty depending on the target tile (number of skull)
                        int difficulty = labyrinthSkulls[leftPos].difficulty;
                        edges.Add(new Edge(nodes[currentPos], nodes[leftPos], difficulty));

                        difficulty = labyrinthSkulls[currentPos].difficulty;
                        edges.Add(new Edge(nodes[leftPos], nodes[currentPos], difficulty));
                    }
                        
                }

                if (y > 0)
                {
                    int downPos = currentPos - labSize;
                    Tile downTile = labyrinthTiles[downPos];
                    if (currentTile.down && downTile.up)
                    {
                        int difficulty = labyrinthSkulls[downPos].difficulty;
                        edges.Add(new Edge(nodes[currentPos], nodes[downPos], difficulty));

                        difficulty = labyrinthSkulls[currentPos].difficulty;
                        edges.Add(new Edge(nodes[downPos], nodes[currentPos], difficulty));
                    }
                        
                }
            }
        }

        Node start = nodes[enemy.pos];
        Debug.Log("enemy pos " + enemy.pos);
        Node target = nodes[player.pos];
        Debug.Log("player pos " + player.pos);

        //Setting graph and calculing shortest path
        DirectedGraph graph = new DirectedGraph(nodes, edges, true);

        Stack<Node> path = new Stack<Node>();
        GraphOperation.getShortestPath(graph, start, target, out path);

        //Calculing path for charcter and tracing it
        List<Vector3> vectorPath = new List<Vector3>();
        while (path.Count != 0)
        {
            Node node = path.Pop();
            int nodeInt = -1;
            if (System.Int32.TryParse(node.label, out nodeInt))
                vectorPath.Add(GetTilePosFromIndex(nodeInt));
        }
        trace.SetPath(vectorPath);
    }



    Vector3 GetTilePosFromIndex(int tile)
    {
        Vector2Int coord = GetTileCoordinatesFromIndex(tile);

        float xpos = (-labSize / 2 + coord.x) * tileSize;
        float ypos = (-labSize / 2 + coord.y) * tileSize;

        return new Vector3(xpos, ypos, 0);
    }

    Vector2Int GetTileCoordinatesFromIndex(int tile)
    {
        int x = tile % labSize;
        int y = (tile - x) / labSize;
        return new Vector2Int(x, y);
    }

    int GetTileIndexFromCoordinates(Vector2Int coord)
    {
        return coord.y * labSize + coord.x;
    }

    Tile FindRandomTileInList(List<Tile> tiles)
    {
        int tileChoice = Random.Range(0, tiles.Count);
        Tile tile = tiles[tileChoice];
        return tile;
    }

    Tile FindRandomTile()
    {
        int tileChoice = Random.Range(0, tilesPrefab.Length);
        Tile tile = tilesPrefab[tileChoice];
        return tile;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Dijkstra;

namespace DijkstraTest
{
    public class TestDijkstra : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            //TEST1
            List<Node> nodes = new List<Node>(new Node[] { new Node("A"), new Node("B"), new Node("C"), new Node("D"), new Node("E"), new Node("F"), new Node("G"), new Node("H"), new Node("I"), new Node("J") });
            List<Edge> edges = new List<Edge>(new Edge[] {
            new Edge(nodes[0], nodes[1], 85),
            new Edge(nodes[0], nodes[2], 217),
            new Edge(nodes[0], nodes[4], 173),
            new Edge(nodes[1], nodes[5], 80),
            new Edge(nodes[2], nodes[6], 186),
            new Edge(nodes[2], nodes[7], 103),
            new Edge(nodes[3], nodes[7], 183),
            new Edge(nodes[4], nodes[9], 502),
            new Edge(nodes[5], nodes[8], 250),
            new Edge(nodes[7], nodes[9], 167),
            new Edge(nodes[8], nodes[9], 84)
        });

            DirectedGraph graph = new DirectedGraph(nodes, edges, false);
            TestGraph(graph, nodes[0], nodes[9]);


            //TEST 2 
            nodes = new List<Node>(new Node[] { new Node("0"), new Node("1"), new Node("2"), new Node("3"), new Node("4"), new Node("5"), new Node("6"), new Node("7") });
            edges = new List<Edge>(new Edge[] {
            new Edge(nodes[0], nodes[1], 5),
            new Edge(nodes[0], nodes[4], 9),
            new Edge(nodes[0], nodes[7], 8),
            new Edge(nodes[1], nodes[2], 12),
            new Edge(nodes[1], nodes[3], 15),
            new Edge(nodes[1], nodes[7], 4),
            new Edge(nodes[2], nodes[3], 3),
            new Edge(nodes[2], nodes[6], 11),
            new Edge(nodes[3], nodes[6], 9),
            new Edge(nodes[4], nodes[5], 4),
            new Edge(nodes[4], nodes[6], 20),
            new Edge(nodes[4], nodes[7], 5),
            new Edge(nodes[5], nodes[2], 1),
            new Edge(nodes[5], nodes[6], 13),
            new Edge(nodes[7], nodes[2], 7),
            new Edge(nodes[7], nodes[5], 6)
        });

            graph = new DirectedGraph(nodes, edges, true);
            TestGraph(graph, nodes[0], nodes[6]);


            //TEST 3
            graph = new DirectedGraph(15, 100);
            TestGraph(graph, graph.GetNode("0"), graph.GetNode("14"));
        }

        public static int TestGraph(DirectedGraph graph, Node start, Node target)
        {
            Debug.LogFormat("Graph = {0}", graph);

            Dictionary<Node, Edge> path = GraphOperation.getShortestPathDistances(graph, start, target);

            if (graph == null || !graph.Contains(start) || !graph.Contains(target))
            {
                Debug.LogFormat("Error param");
                return -1;
            }

            if (path == null)
            {
                Debug.LogFormat("No solution");
                return -1;
            }

            Node currentNode = target;
            string displayPath = currentNode.ToString();

            while (currentNode != start)
            {
                Edge edge = path[currentNode];
                currentNode = edge.src;
                displayPath = currentNode + "->" + displayPath;
            }

            Debug.LogFormat(displayPath + " : " + path[target].weight);
            return path[target].weight;

        }
    }
}

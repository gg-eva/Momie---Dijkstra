using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dijkstra
{
    public class DirectedGraph
    {
        private Dictionary<Node, HashSet<Edge>> graphMap; //Map of nodes and edges from them

        private const int RDM_MAX_NODE = 1000;

        //Empty graph generation
        public DirectedGraph()
        {
            graphMap = new Dictionary<Node, HashSet<Edge>>();
        }

        //Random graph generation
        public DirectedGraph(int nbNode, int maxEdgeValue)
        {
            graphMap = new Dictionary<Node, HashSet<Edge>>();

            //Creates each node
            List<Node> nodes = new List<Node>();
            for (int i = 0; i < nbNode; ++i)
            {
                Node node = new Node(i.ToString());
                //Easier to manip
                nodes.Add(node);
                //Add to graph
                this.Add(node);
            }

            //For each node, it creates a random number of edge between 1 and log10n, and give it a random value
            int maxNbEdge = (int)Mathf.Sqrt(nbNode);
            for (int i = 0; i < nbNode; ++i)
            {
                Node node = nodes[i];
                List<Node> possibleNode = new List<Node>(nodes);
                possibleNode.Remove(nodes[i]);

                int randNE = UnityEngine.Random.Range(1, maxNbEdge);
                for (int j = 0; j < randNE; ++j)
                {
                    //Get random end (only one edge between a source and an end
                    int randN = UnityEngine.Random.Range(0, possibleNode.Count);
                    Node randNode = possibleNode[randN];
                    //Get random value for the edge
                    int randV = UnityEngine.Random.Range(1, maxEdgeValue);
                    Edge edge = new Edge(node, randNode, randV);
                    //Add edge to graph
                    this.AddEdge(node, edge);
                    possibleNode.Remove(randNode);
                }
            }
        }

        //Graph generation from data
        public DirectedGraph(List<Node> nodes, List<Edge> edges, bool directed)
        {
            graphMap = new Dictionary<Node, HashSet<Edge>>();

            foreach (Node node in nodes)
            {
                this.Add(node);
            }

            foreach (Edge edge in edges)
            {
                this.AddEdge(edge.src, edge);
                //Undirected graph ? Add reverse directed edge
                if (!directed) this.AddEdge(edge.end, new Edge(edge.end, edge.src, edge.weight));
            }
        }

        public bool Contains(Node node)
        {
            return graphMap.ContainsKey(node);
        }

        public HashSet<Edge> GetEdges(Node node)
        {
            return graphMap[node];
        }

        public Node GetNode(string name)
        {
            foreach (Node node in graphMap.Keys)
                if (node.label == name)
                    return node;
            return null;
        }

        public void Add(Node node)
        {
            graphMap.Add(node, new HashSet<Edge>());
        }

        public void AddEdge(Node node, Edge edge)
        {
            HashSet<Edge> edges = graphMap[node];
            edges.Add(edge);
            graphMap[node] = edges;
        }

        public override string ToString()
        {
            string result = "[ ";

            foreach (Node node in graphMap.Keys)
            {
                result += "[ " + node + " : ";
                foreach (Edge edge in graphMap[node])
                    result += edge + " ";
                result += "]";
            }
            result += "]";
            return result;
        }
    }

    public class Node
    {
        public readonly string label;

        public Node(string label)
        {
            this.label = label;
        }

        public override string ToString()
        {
            return label;
        }
    }

    public class Edge
    {
        public int weight { get; set; }
        public Node src { get; set; }
        public Node end { get; set; }

        public Edge(Node src, Node end, int weight)
        {
            this.src = src;
            this.end = end;
            this.weight = weight;
        }

        public override string ToString()
        {
            return "[" + src + "->" + end + "-" + weight + "]";
        }
    }

    public static class GraphOperation
    {
        public static Dictionary<Node, Edge> getShortestPathDistances(DirectedGraph graph, Node start, Node target)
        {
            //check parameters
            if (graph == null || start == null || target == null)
                return null;

            //check if node in graph
            if (!graph.Contains(start) || !graph.Contains(target))
                return null;

            HashSet<Node> nodesToCheck = new HashSet<Node>();
            nodesToCheck.Add(start);

            Dictionary<Node, Edge> subGraph = new Dictionary<Node, Edge>();

            Node currentNode = start;
            Edge currentdge = new Edge(start, start, 0);
            subGraph[start] = currentdge;

            while (currentNode != target)
            {
                //Add possible paths from current node
                HashSet<Edge> setOfEdges = graph.GetEdges(currentNode);

                //Check around the current node, update length from source
                foreach (Edge edge in setOfEdges)
                {
                    Node endNode = edge.end;

                    int endWeight = subGraph[currentNode].weight + edge.weight;
                    if (subGraph.ContainsKey(endNode))
                    {
                        if (endWeight < subGraph[endNode].weight)
                        {
                            Edge newEdge = new Edge(currentNode, endNode, endWeight);
                            subGraph[endNode] = newEdge;
                        }
                    }
                    else
                    {
                        Edge newEdge = new Edge(currentNode, endNode, endWeight);
                        subGraph[endNode] = newEdge;
                        nodesToCheck.Add(endNode);
                    }
                }

                //The node has been checked
                nodesToCheck.Remove(currentNode);

                //Next node is the nearest
                int smallestWeight = int.MaxValue;
                foreach (Node node in nodesToCheck)
                {
                    if (subGraph[node].weight < smallestWeight)
                    {
                        currentNode = node;
                        smallestWeight = subGraph[currentNode].weight;
                    }
                }

                //If empty, no solution
                if (nodesToCheck.Count == 0)
                    return null;

                //If next node is the end, then the shortest path is found
            }

            return subGraph;
        }

        public static int getShortestPath(DirectedGraph graph, Node start, Node target, out Stack<Node> path)
        {
            path = new Stack<Node>();

            Dictionary<Node, Edge> shortestPath = GraphOperation.getShortestPathDistances(graph, start, target);

            if (shortestPath == null)
            {
                return -1;
            }

            Node currentNode = target;
            path.Push(currentNode);

            while (currentNode != start)
            {
                Edge edge = shortestPath[currentNode];
                currentNode = edge.src;
                path.Push(currentNode);
            }


            return shortestPath[target].weight;
        }
    }
}
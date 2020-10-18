using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AI_TP1
{
    public class PathFinder
    {
        public List<Node> Dijkstra(Node start, Node end, Node[,] mat_node)
        {
            List<Node> path = new List<Node>();
            if (start == end)
            {
                path.Add(start);
                return path;
            }

            List<Node> unvisited = new List<Node>();

            Dictionary<Node, Node> previous = new Dictionary<Node, Node>();

            Dictionary<Node, float> distances = new Dictionary<Node, float>();

            foreach (Node n in mat_node)
            {
                unvisited.Add(n);

                distances.Add(n, float.MaxValue);
            }

            distances[start] = 0f;

            while (unvisited.Count != 0)
            {

                unvisited = unvisited.OrderBy(node => distances[node]).ToList();

                Node current = unvisited[0];

                unvisited.Remove(current);

                if (current == end)
                {

                    while (previous.ContainsKey(current))
                    {

                        path.Insert(0, current);

                        current = previous[current];
                    }

                    path.Insert(0, current);
                    break;
                }

                foreach (Node neighbor in current.GetNeightboors())
                {
                    float alt = distances[current] + neighbor.GetWeight();

                    if (alt < distances[neighbor])
                    {
                        distances[neighbor] = alt;
                        previous[neighbor] = current;
                    }
                }
            }
            return path;
        }

        public List<Node> Astar(Node start, Node end)
        {

            List<Node> open_list = new List<Node>();
            List<Node> closed_list = new List<Node>();


            open_list.Add(start);

            while (open_list.Count > 0)
            {
                Node current_node = open_list[0];
                for (int i = 1; i < open_list.Count; i++)
                {
                    if (open_list[i].node_cost < current_node.node_cost || open_list[i].node_cost == current_node.node_cost && open_list[i].dist_to_end < current_node.dist_to_end)
                    {
                        current_node = open_list[i];
                    }
                }
                open_list.Remove(current_node);
                closed_list.Add(current_node);

                if (current_node == end)
                {
                    GetFinalPath(start, end);
                }

                foreach (Node neighbor in current_node.GetNeightboors())
                {
                    if (closed_list.Contains(neighbor))
                    {
                        continue;
                    }
                    int MoveCost = current_node.dist_from_start + current_node.GetWeight() + GetManhattenDistance(current_node, neighbor);

                    if (MoveCost < (neighbor.dist_from_start + neighbor.GetWeight()) || !open_list.Contains(neighbor))
                    {
                        neighbor.dist_from_start = MoveCost;
                        neighbor.dist_to_end = GetManhattenDistance(neighbor, end);
                        neighbor.parent = current_node;

                        if (!open_list.Contains(neighbor))
                        {
                            open_list.Add(neighbor);
                        }
                    }
                }
            }
            return GetFinalPath(start, end);
        }

        List<Node> GetFinalPath(Node a_StartingNode, Node a_EndNode)
        {
            List<Node> FinalPath = new List<Node>();
            Node CurrentNode = a_EndNode;

            while (CurrentNode != a_StartingNode)
            {
                FinalPath.Add(CurrentNode);
                if (CurrentNode.parent == null)
                {
                    break;
                }
                else
                {
                    CurrentNode = CurrentNode.parent;
                }
            }

            FinalPath.Reverse();

            return FinalPath;

        }

        private int GetManhattenDistance(Node n1, Node n2)
        {
            int ix = Mathf.Abs(n1.GetX() - n2.GetX());
            int iy = Mathf.Abs(n1.GetY() - n2.GetY());

            return ix + iy;
        }

    }
}
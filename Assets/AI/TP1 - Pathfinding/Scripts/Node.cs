using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace AI_TP1
{
    public class Node
    {
        // Coordinates
        private int x;
        private int y;

        public Node parent;

        private int weight;

        public int dist_from_start;
        public int dist_to_end;
        public int node_cost { get { return dist_from_start + dist_to_end; } }

        private List<Node> neightboor = new List<Node>();

        public Node(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.weight = 0;
        }

        public int GetX()
        {
            return this.x;
        }

        public int GetY()
        {
            return this.y;
        }

        public List<Node> GetNeightboors()
        {
            return this.neightboor;
        }

        public Node GetRandomNeightboors()
        {
            return this.neightboor[Random.Range(0, this.neightboor.Count)];
        }

        public int GetWeight()
        {
            return this.weight;
        }

        public void SetWeight(int w)
        {
            this.weight = w;
        }

        public void AddNeightboor(Node n)
        {
            this.neightboor.Add(n);
        }

        public bool Equal(Node n)
        {
            return GetX() == n.GetX() && GetY() == n.GetY() && GetWeight() == n.GetWeight();
        }

    }
}
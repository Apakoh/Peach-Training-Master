using UnityEngine;
using Random = UnityEngine.Random;

namespace AI_TP1
{
    public class GraphCreator
    {
        // Play Area Size
        public int size = 30;

        //Min & Max
        public int min = 1;
        public int max = 1;

        public int prob_wall = 5;

        // Matrice d'adjacence du graphe
        public Node[,] mat_graph;

        //APIs
        private MatriceAPI mat_api;

        public bool done = false;

        public GraphCreator(int s, int prob_wall)
        {
            this.size = s;
            this.prob_wall = prob_wall;
        }

        public void Creation()
        {
            this.mat_api = new MatriceAPI();
            InitiateMatriceGraph();
            CreateNodes();
            FillMatriceGraph();
            this.done = true;
        }

        private void InitiateMatriceGraph()
        {
            this.mat_graph = new Node[this.size, this.size];
        }

        private void CreateNodes()
        {
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    Node nodetemp = new Node(j, i);
                    nodetemp.SetWeight(RandomValue(min, max));
                    this.mat_graph[i, j] = nodetemp;
                }
            }
        }

        private void FillMatriceGraph()
        {
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    // Case adjacente (droite)
                    if ((!RandomWallChoice() && j < this.size - 1) || (i == 0 && j == 0))
                    {
                        this.mat_graph[i, j].AddNeightboor(this.mat_graph[i, j + 1]);
                        this.mat_graph[i, j + 1].AddNeightboor(this.mat_graph[i, j]);

                        // Premier Node
                        Vector3 vect1 = this.mat_api.MattoVect3(i, j, this.mat_graph);
                        // Second Node
                        Vector3 vect2 = this.mat_api.MattoVect3(i, j + 1, this.mat_graph);
                    }

                    // Case adjacente (bas)
                    if ((!RandomWallChoice() && i < this.size - 1) || (i == 0 && j == 0))
                    {
                        this.mat_graph[i, j].AddNeightboor(this.mat_graph[i + 1, j]);
                        this.mat_graph[i + 1, j].AddNeightboor(this.mat_graph[i, j]);

                        // Premier Node
                        Vector3 vect1 = this.mat_api.MattoVect3(i, j, this.mat_graph);
                        // Second Node
                        Vector3 vect2 = this.mat_api.MattoVect3(i + 1, j, this.mat_graph);
                    }
                }
            }
        }

        private bool RandomWallChoice()
        {
            return Random.Range(0, this.prob_wall) == 0;
        }

        private int RandomValue(int min, int max)
        {
            return Random.Range(min, max);
        }

    }
}
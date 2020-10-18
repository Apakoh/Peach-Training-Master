using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_TP1
{
    public class GameMasterAITP1 : MonoBehaviour
    {
        public Player prefab_player;
        public Enemy prefab_enemy;


        private GraphCreator graph_creator;

        public Node[,] mat_graph;
        public PathFinder pathfinder;
        public MatriceAPI mat_api;
        public LineManager line_manager;

        [Range(3, 100)]
        public int map_size = 30;
        [Range(4, 10)]
        public int prob_wall = 5;

        public int x_end = 2;
        public int y_end = 2;
        public bool player_auto;

        [Range(1f, 10f)]
        public float player_speed = 5f;
        [Range(1f, 10f)]
        public float enemy_speed = 3f;
        [Range(1, 5)]
        public int enemy_number = 3;

        public Player player_controller;
        public List<Enemy> enemies_controller = new List<Enemy>();

        private bool reset_cd = false;

        private void Start()
        {
            this.graph_creator = new GraphCreator(this.map_size, this.prob_wall);
            this.line_manager = this.GetComponent<LineManager>();
            this.mat_api = new MatriceAPI();
            this.pathfinder = new PathFinder();

            PopulateMap();
            StartGame();
        }

        private void Update()
        {
            if (!reset_cd)
            {
                StartCoroutine(ResetMap());
            }
        }

        private void StartGame()
        {
            this.graph_creator.Creation();
            StartCoroutine(WaitMap());
        }

        private void Game()
        {
            this.mat_graph = this.graph_creator.mat_graph;
            this.line_manager.gm = this;

            this.line_manager.DrawMap();
        }

        private void PopulateMap()
        {
            this.player_controller = Instantiate(this.prefab_player, Vector3.zero, Quaternion.Euler(0f, 0f, 0f));
            this.player_controller.gm = this;
            this.player_controller.name = "Player";

            for (int i = 0; i < this.enemy_number; i++)
            {
                Vector3 vect_temp_enemy = new Vector3(RandomCoordinate(), 0, RandomCoordinate());
                Enemy enemy = Instantiate(this.prefab_enemy, vect_temp_enemy, Quaternion.Euler(0f, 0f, 0f));
                enemy.name = "Enemy " + i;
                enemy.gm = this;
                enemy.player = this.player_controller;
                this.enemies_controller.Add(enemy);
            }
        }

        private int RandomCoordinate()
        {
            return Mathf.RoundToInt(Random.Range(0, this.map_size - 1));
        }

        private IEnumerator WaitMap()
        {
            while (!this.graph_creator.done) { }
            Game();
            yield return null;
        }

        private IEnumerator ResetMap()
        {
            this.reset_cd = true;
            yield return new WaitForSeconds(3);
            this.reset_cd = false;
            StartGame();
            this.player_controller.PlayerAuto();
        }
    }
}
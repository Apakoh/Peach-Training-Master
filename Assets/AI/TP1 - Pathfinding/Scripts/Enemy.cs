using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_TP1
{
    public class Enemy : CharacterController
    {
        public Player player;

        private bool move_wait = false;

        void Start()
        {
            Initialisation();
            this.gm.pathfinder = new PathFinder();
        }

        void Update()
        {
            this.speed_character = this.gm.enemy_speed;

            if (!this.move_wait)
            {
                MoveToPlayer();
                MoveAlongPath();
                //StartCoroutine(MoveWait());
            }
        }

        private void MoveToPlayer()
        {
            if (this.gm.mat_graph != null)
            {
                Node n_enemy = this.gm.mat_graph[Mathf.RoundToInt(this.transform.position.z), Mathf.RoundToInt(this.transform.position.x)];
                Node n_player = this.gm.mat_graph[Mathf.RoundToInt(this.player.transform.position.z), Mathf.RoundToInt(this.player.transform.position.x)];
                List<Node> path = this.gm.pathfinder.Astar(n_enemy, n_player);
                this.SetPath(path);
            }
        }

        private IEnumerator MoveWait()
        {
            this.move_wait = true;
            yield return new WaitForSeconds(0.1f);
            this.move_wait = false;
        }
    }
}
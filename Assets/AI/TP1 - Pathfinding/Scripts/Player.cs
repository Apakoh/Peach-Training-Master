using System.Collections.Generic;
using UnityEngine;

namespace AI_TP1
{
    public class Player : CharacterController
    {
        public bool player_auto;
        public bool running;

        private void Start()
        {
            Initialisation();
            this.player_auto = this.gm.player_auto;
            this.running = false;
        }

        void Update()
        {
            this.speed_character = this.gm.player_speed;
            this.player_auto = this.gm.player_auto;

            if (this.player_auto)
            {
                if (!this.running)
                {
                    PlayerAuto();
                    this.running = true;
                }

                MoveAlongPath();
            }
            else
            {
                ControlPlayer();
            }
        }

        private void ControlPlayer()
        {
            RemovePath();
            this.running = false;

            Vector3 temp_vect = Vector3.zero;

            if (Input.GetKey(KeyCode.Z))
            {
                temp_vect = new Vector3(0, 0, 1);
            }
            if (Input.GetKey(KeyCode.S))
            {
                temp_vect = new Vector3(0, 0, -1);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                temp_vect = new Vector3(-1, 0, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                temp_vect = new Vector3(1, 0, 0);
            }

            temp_vect = temp_vect.normalized * 20 * Time.deltaTime + this.transform.position;
            this.Move(temp_vect);
        }

        public void PlayerAuto()
        {
            this.current_direction = this.transform.position;
            Node n_player = this.gm.mat_graph[Mathf.RoundToInt(this.transform.position.z), Mathf.RoundToInt(this.transform.position.x)];
            List<Node> path = this.gm.pathfinder.Dijkstra(n_player, this.gm.mat_graph[this.gm.x_end - 1, this.gm.y_end - 1], this.gm.mat_graph);
            this.RemovePath();
            this.SetPath(path);
        }
    }
}
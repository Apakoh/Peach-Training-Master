using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace AI_TP2
{
    public class GameMasterTP2 : MonoBehaviour
    {
        public Agent agent_prefab;
        public GameObject obstacle_prefab;

        public GameObject agents_parent;
        public GameObject obstacle_parent;

        public AI_Boids_Player player;

        public List<GameObject> targets = new List<GameObject>();

        public List<Agent> agents = new List<Agent>();
        public List<GameObject> obstacles = new List<GameObject>();

        public Vector3 center_structure = new Vector3(0, 0, 0);
        [Range(1f, 150f)]
        public float size_side_X = 50f;
        [Range(1f, 150f)]
        public float size_side_Y = 25f;
        [Range(1f, 150f)]
        public float size_side_Z = 50f;

        public float[] weights;

        [Range(1, 500)]
        public int flock_number = 250;

        [Range(1f, 100f)]
        public float flock_overall_speed = 10f;
        [Range(1f, 100f)]
        public float speed_max = 5f;

        [Range(1f, 50f)]
        public float neighbor_radius = 1f;
        [Range(0.1f, 20f)]
        public float avoidance_radius = 0.5f;

        [Range(0.1f, 5f)]
        public float smooth_time_cohesion = 0.5f;
        [Range(0.001f, 0.5f)]
        public float smooth_time = 0.5f;

        [Range(15f, 100f)]
        public float radius_area = 15f;

        [Range(0, 50)]
        public int obstacle_number = 0;
        private float obstacle_radius;

        float square_max_speed;
        public float GetSquareMaxSpeed { get { return this.square_max_speed; } }

        public StateMachine state_machine;

        private void Start()
        {
            this.square_max_speed = this.speed_max * this.speed_max;

            this.obstacle_radius = this.obstacle_prefab.GetComponent<SphereCollider>().radius;

            this.state_machine = this.GetComponent<StateMachine>();

            for (int obs = 0; obs < obstacle_number; obs++)
            {
                Vector3 random_position_obstacle = GetRandomSpawnObstacle();
                GameObject new_obstacle = Instantiate(this.obstacle_prefab, random_position_obstacle, Quaternion.Euler(Vector3.zero), this.obstacle_parent.transform);
                new_obstacle.name = "Obstacle " + obs;
                obstacles.Add(new_obstacle);
            }

            for (int i = 0; i < flock_number; i++)
            {
                Vector3 random_position_agent = GetRandomSpawnAgent();
                Agent new_agent = Instantiate(this.agent_prefab, random_position_agent, Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)), this.agents_parent.transform);
                new_agent.name = "Agent " + i;
                new_agent.gm = this;
                agents.Add(new_agent);
            }
        }


        public bool AgentSpawnAttempt(Vector3 position, float radius_to_check, string[] list_to_check)
        {
            Collider[] colliders = Physics.OverlapSphere(position, radius_to_check);
            bool valid_position = true;

            foreach (Collider col in colliders)
            {
                foreach (string element in list_to_check)
                {
                    if (col.tag == element)
                    {
                        valid_position = false;
                    }
                }
            }

            return valid_position;
        }

        private Vector3 GetRandomSpawnObstacle()
        {
            Vector3 random_position_obstacle = new Vector3(Random.Range(-this.size_side_X, this.size_side_X), Random.Range(-this.size_side_Y, this.size_side_Y), Random.Range(-this.size_side_Z, this.size_side_Z));

            string[] list_tag = { "obstacles" };

            while (!AgentSpawnAttempt(random_position_obstacle, this.obstacle_radius, list_tag))
            {
                random_position_obstacle = new Vector3(Random.Range(-this.size_side_X, this.size_side_X), Random.Range(-this.size_side_Y, this.size_side_Y), Random.Range(-this.size_side_Z, this.size_side_Z));
            }

            return random_position_obstacle;
        }

        private Vector3 GetRandomSpawnAgent()
        {
            Vector3 random_position_agent = new Vector3(Random.Range(-this.size_side_X, this.size_side_X), Random.Range(0, this.size_side_Y), Random.Range(-this.size_side_Z, this.size_side_Z));

            string[] list_tag = { "obstacles", "agent" };

            while (!AgentSpawnAttempt(random_position_agent, this.neighbor_radius, list_tag))
            {
                random_position_agent = new Vector3(Random.Range(-this.size_side_X, this.size_side_X), Random.Range(-this.size_side_Y, this.size_side_Y), Random.Range(-this.size_side_Z, this.size_side_Z));
            }

            return random_position_agent;
        }
    }
}
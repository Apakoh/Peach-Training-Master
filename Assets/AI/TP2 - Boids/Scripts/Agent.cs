using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_TP2
{
    [RequireComponent(typeof(SphereCollider))]
    public class Agent : MonoBehaviour
    {
        public List<GameObject> neighbors = new List<GameObject>();
        public List<GameObject> obstacles = new List<GameObject>();

        public AgentBehavior behavior;

        public GameMasterTP2 gm;

        private SphereCollider agent_col;
        public SphereCollider GetAgentCollider { get { return this.agent_col; } }

        public bool can_be_killed = false;

        private void Start()
        {
            this.agent_col = GetComponent<SphereCollider>();
        }

        public void Move(Vector3 velocity)
        {
            this.transform.up = velocity;
            this.transform.position += velocity * Time.deltaTime;
        }

        private void Update()
        {
            this.agent_col.radius = this.gm.neighbor_radius;

            Vector3 move = behavior.NextMove(this, this.gm);
            move *= this.gm.flock_overall_speed;

            if (move.sqrMagnitude > this.gm.GetSquareMaxSpeed)
            {
                move = move.normalized * this.gm.speed_max;
            }

            Move(move);
        }

        // Collider Gestion
        void OnTriggerEnter(Collider col)
        {
            if (col.tag == "Agent")
                neighbors.Add(col.gameObject);
            else if (col.tag == "Obstacle")
                obstacles.Add(col.gameObject);
        }

        void OnTriggerExit(Collider col)
        {
            if (col.tag == "Agent")
                neighbors.Remove(col.gameObject);
            else if (col.tag == "Obstacle")
                obstacles.Remove(col.gameObject);
        }
    }
}
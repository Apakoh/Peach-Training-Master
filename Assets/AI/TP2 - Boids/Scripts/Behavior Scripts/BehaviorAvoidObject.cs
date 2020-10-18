using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_TP2
{
    [CreateAssetMenu(menuName = "Boids/Behavior/Avoid Obstacles")]
    public class BehaviorAvoidObject : AgentBehavior
    {
        private Vector3 current_velocity;

        public override Vector3 NextMove(Agent agent, GameMasterTP2 gm)
        {
            if (agent.obstacles.Count == 0)
            {
                return Vector3.zero;
            }

            Vector3 move_to_avoid = Vector3.zero;

            foreach (GameObject obstacle in agent.obstacles)
            {
                if (CheckFront(agent))
                {
                    move_to_avoid += -agent.transform.up;
                }
            }

            // move_to_avoid = Vector3.SmoothDamp(agent.transform.up, move_to_avoid, ref current_velocity, gm.smooth_time);

            return move_to_avoid;
        }

        private bool CheckFront(Agent agent)
        {
            LayerMask mask = LayerMask.GetMask("Obstacle");
            return Physics.Raycast(agent.transform.position, agent.transform.up.normalized, agent.gm.neighbor_radius * 4, mask);
        }
    }
}
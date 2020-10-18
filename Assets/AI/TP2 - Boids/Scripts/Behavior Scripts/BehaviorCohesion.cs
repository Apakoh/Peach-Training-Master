using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_TP2
{
    [CreateAssetMenu(menuName = "Boids/Behavior/Cohesion")]
    public class BehaviorCohesion : AgentBehavior
    {
        Vector3 current_velocity;

        public override Vector3 NextMove(Agent agent, GameMasterTP2 gm)
        {
            if (agent.neighbors.Count == 0)
            {
                return Vector3.zero;
            }

            Vector3 move_cohesion = Vector3.zero;

            foreach (GameObject neighbor_agent in agent.neighbors)
            {
                if (neighbor_agent != null)
                    move_cohesion += neighbor_agent.transform.position;
            }

            move_cohesion /= agent.neighbors.Count;
            move_cohesion -= agent.transform.position;
            move_cohesion = Vector3.SmoothDamp(agent.transform.up, move_cohesion, ref current_velocity, agent.gm.smooth_time_cohesion);

            return move_cohesion;
        }
    }
}
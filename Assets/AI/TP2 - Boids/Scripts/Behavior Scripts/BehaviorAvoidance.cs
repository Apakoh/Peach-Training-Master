using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_TP2
{
    [CreateAssetMenu(menuName = "Boids/Behavior/Avoidance")]
    public class BehaviorAvoidance : AgentBehavior
    {
        public override Vector3 NextMove(Agent agent, GameMasterTP2 gm)
        {
            if (agent.neighbors.Count == 0)
            {
                return Vector3.zero;
            }

            Vector3 move_avoidance = Vector3.zero;
            int nAvoid = 0;

            foreach (GameObject neighbor_agent in agent.neighbors)
            {
                if (neighbor_agent != null)
                {
                    if (Vector3.Distance(neighbor_agent.transform.position, agent.transform.position) < gm.avoidance_radius)
                    {
                        nAvoid++;
                        move_avoidance += agent.transform.position - neighbor_agent.transform.position;
                    }
                }
            }
            if (nAvoid > 0)
            {
                move_avoidance /= nAvoid;
            }

            return move_avoidance;
        }
    }
}
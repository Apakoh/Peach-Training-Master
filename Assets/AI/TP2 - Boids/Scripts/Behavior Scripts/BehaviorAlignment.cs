using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_TP2
{
    [CreateAssetMenu(menuName = "Boids/Behavior/Alignment")]
    public class BehaviorAlignment : AgentBehavior
    {
        public override Vector3 NextMove(Agent agent, GameMasterTP2 gm)
        {
            if (agent.neighbors.Count == 0)
            {
                return Vector3.zero;
            }

            Vector3 move_alignment = Vector3.zero;
            foreach (GameObject neighbor_agent in agent.neighbors)
            {
                if (neighbor_agent != null)
                    move_alignment += neighbor_agent.transform.up;
            }
            move_alignment /= agent.neighbors.Count;

            return move_alignment;
        }
    }
}
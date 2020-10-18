using System.Collections.Generic;
using UnityEngine;

namespace AI_TP2
{
    [CreateAssetMenu(menuName = "Boids/Behavior/Global")]
    public class BehaviorGlobal : AgentBehavior
    {
        public AgentBehavior[] behaviors;
        public float[] weights;

        public override Vector3 NextMove(Agent agent, GameMasterTP2 gm)
        {
            this.weights = agent.gm.weights;

            if (weights.Length != behaviors.Length)
            {
                Debug.LogError("Data problem in " + name, this);
                return Vector3.zero;
            }

            Vector3 move_agent = Vector3.zero;

            Vector3 move_temp;

            for (int i = 0; i < behaviors.Length; i++)
            {
                move_temp = behaviors[i].NextMove(agent, gm) * weights[i];

                if (move_temp != Vector3.zero)
                {
                    if (move_temp.sqrMagnitude > weights[i] * weights[i])
                    {
                        move_temp.Normalize();
                        move_temp *= weights[i];
                    }

                    move_agent += move_temp;
                }
            }

            if (move_agent == Vector3.zero)
            {
                move_agent = agent.transform.up;
            }

            return move_agent;
        }
    }
}
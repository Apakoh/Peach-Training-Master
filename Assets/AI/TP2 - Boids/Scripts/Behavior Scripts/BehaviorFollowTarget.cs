using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_TP2
{
    [CreateAssetMenu(menuName = "Boids/Behavior/Follow Target")]
    public class BehaviorFollowTarget : AgentBehavior
    {
        public override Vector3 NextMove(Agent agent, GameMasterTP2 gm)
        {
            if (gm.targets.Count == 0)
            {
                return Vector3.zero;
            }

            GameObject closest_target = gm.targets[0];
            float distance_min = Vector3.Distance(closest_target.transform.position, agent.transform.position);
            float distance_min_temp;

            for (int i = 1; i < gm.targets.Count; i++)
            {
                distance_min_temp = Vector3.Distance(gm.targets[i].transform.position, agent.transform.position);
                if (distance_min_temp < distance_min)
                {
                    distance_min = distance_min_temp;
                    closest_target = gm.targets[i];
                }
            }

            Vector3 move_to_target = (closest_target.transform.position - agent.transform.position).normalized;

            return move_to_target;
        }
    }
}

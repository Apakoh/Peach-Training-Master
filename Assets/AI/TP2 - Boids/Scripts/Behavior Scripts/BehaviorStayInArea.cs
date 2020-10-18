using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_TP2
{
    [CreateAssetMenu(menuName = "Boids/Behavior/Stay In Area")]
    public class BehaviorStayInArea : AgentBehavior
    {
        private Vector3 current_velocity;

        public override Vector3 NextMove(Agent agent, GameMasterTP2 gm)
        {
            float size_X = gm.size_side_X;
            float size_Y = gm.size_side_Y;
            float size_Z = gm.size_side_Z;
            Vector3 center = gm.center_structure;

            //Inside given square of Side Length radius
            float x = agent.transform.position.x;
            bool x_bool = -size_X < x && x < size_X;
            float y = agent.transform.position.y;
            bool y_bool = -size_Y < y && y < size_Y;
            float z = agent.transform.position.z;
            bool z_bool = -size_Z < z && z < size_Z;

            if (x_bool && y_bool && z_bool)
            {
                return Vector3.zero;
            }

            Vector3 center_direction = (center - agent.transform.position).normalized;


            Vector3 final_direction = Vector3.SmoothDamp(agent.transform.up, center_direction, ref current_velocity, agent.gm.smooth_time);

            return final_direction;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_TP2
{
    [CreateAssetMenu(menuName = "Boids/Behavior/Stay In Radius")]
    public class BehabiorStayInRadius : AgentBehavior
    {
        public Vector3 center;

        public override Vector3 NextMove(Agent agent, GameMasterTP2 gm)
        {
            float radius = gm.radius_area;

            //Inside given Sphere of given radius
            Vector3 center_offset = center - agent.transform.position;
            float t = center_offset.magnitude / radius;

            if (t < 0.9f)
            {
                return Vector3.zero;
            }

            return center_offset * t * t;
        }
    }
}
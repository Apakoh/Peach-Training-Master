using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthese_TP2
{
    public class Point : MonoBehaviour
    {
        public Vector3 velocity;
        public float mass;

        public Vector3 previous_position;

        private void Start()
        {
            this.velocity = RandomVelocity();
        }

        private Vector3 RandomVelocity()
        {
            return new Vector3(RandomVector3(50f), RandomVector3(50f), RandomVector3(50f));
        }

        private float RandomVector3(float range)
        {
            return Random.Range(0, range);
        }
    }
}
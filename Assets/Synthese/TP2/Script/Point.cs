using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthese_TP2
{
    public class Point : MonoBehaviour
    {
        public Vector3 velocity;
        public float mass;

        private void Start()
        {
            this.velocity = RandomVelocity();
            this.mass = this.GetComponent<Rigidbody>().mass;
        }

        private Vector3 RandomVelocity()
        {
            return new Vector3(RandomVector3(5f), RandomVector3(10f), RandomVector3(5f));
        }

        private float RandomVector3(float range)
        {
            return Random.Range(-range, range);
        }
    }
}
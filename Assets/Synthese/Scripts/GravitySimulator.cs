using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthese
{
    public class GravitySimulator : MonoBehaviour
    {
        private List<Point> list_points = new List<Point>();

        private Vector3 gravity = new Vector3(0, -9.8f, 0);

        private void Start()
        {
            this.list_points = this.GetComponent<PointFactory>().list_points;

            foreach(Point pt in this.list_points)
            {
                RandomVelocity(pt);
            }
        }

        private void Update()
        {
            foreach (Point pt in this.list_points)
            {
                Gravity(pt);
            }
        }

        private void Gravity(Point pt)
        {
            pt.transform.position += pt.velocity + gravity;
        }

        private void RandomVelocity(Point pt)
        {
            pt.velocity = new Vector3(RandomVector3(5f), RandomVector3(5f), RandomVector3(5f));
        }

        private float RandomVector3(float range)
        {
            return Random.Range(-range, range);
        }

        private Rigidbody GetRigidbodyPoint(Point pt)
        {
            return pt.gameObject.GetComponent<Rigidbody>();
        }
    }
}
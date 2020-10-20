using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthese_TP2
{
    public class GravitySimulator : MonoBehaviour
    {
        private List<Point> list_points = new List<Point>();

        private int bundaries;

        private Vector3 gravity = new Vector3(0, -9.8f, 0);

        private void Start()
        {
            PointFactory pf = this.GetComponent<PointFactory>();
            this.list_points = pf.list_points;
            this.bundaries = pf.bundaries;

            foreach(Point pt in this.list_points)
            {
                RandomVelocity(pt);
            }
        }

        private void Update()
        {
            foreach (Point pt in this.list_points)
            {
                Vector3 pt_nex_pos = Gravity(pt);
                if (!OutOfRange(pt_nex_pos))
                {
                    pt.transform.position = pt_nex_pos;
                }
                else
                {
                    pt.velocity = Vector3.zero;
                }
            }
        }

        private Vector3 Gravity(Point pt)
        {
           return pt.transform.position + pt.velocity + gravity;
        }

        private void RandomVelocity(Point pt)
        {
            pt.velocity = new Vector3(RandomVector3(50f), RandomVector3(50f), RandomVector3(50f));
        }

        private float RandomVector3(float range)
        {
            return Random.Range(-range, range);
        }

        private bool OutOfRange(Vector3 pos)
        {
            bool X = pos.x > this.bundaries || pos.x < -this.bundaries;
            bool Y = pos.y > this.bundaries || pos.y < -this.bundaries;
            bool Z = pos.z > this.bundaries || pos.z < -this.bundaries;

            return X || Y || Z;
        }

    }
}
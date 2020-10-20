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

        private bool gravity_check;

        [Range(0, 10)]
        public float k;

        private void Start()
        {
            PointFactory pf = this.GetComponent<PointFactory>();
            this.list_points = pf.list_points;
            this.bundaries = pf.bundaries;
            this.gravity_check = true;

            Time.timeScale = 1f;
        }

        private void Update()
        {
            if(gravity_check)
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

                StartCoroutine(GravityTimer(0.00001f));
            }
        }

        // GRAVITY SIMULATION

        private Vector3 Gravity(Point pt)
        {
           return pt.transform.position + DeltaTime() * pt.velocity + DeltaTime() * gravity;
        }

        private bool OutOfRange(Vector3 pos)
        {
            bool X = pos.x > this.bundaries || pos.x < -this.bundaries;
            bool Y = pos.y > this.bundaries || pos.y < -this.bundaries;
            bool Z = pos.z > this.bundaries || pos.z < -this.bundaries;

            return X || Y || Z;
        }

        private IEnumerator GravityTimer(float seconds)
        {
            this.gravity_check = false;
            yield return new WaitForSeconds(seconds);
            this.gravity_check = true;
        }

        private float DeltaTime()
        {
            return Time.deltaTime;
        }

        // VISCOELASTIC SIMULATION

        private void ViscoSimulation()
        {
            foreach(Point pt in this.list_points)
            {
                pt.velocity += DeltaTime() * this.gravity;
            }

            // ApplyViscosity();

            foreach(Point pt in this.list_points)
            {
                pt.previous_position = pt.transform.position;
                pt.transform.position += pt.velocity;
            }

            // AdjustSprings();
            // ApplySpringDisplacements();
            DoubleDensityRelaxation();
            // ResolveCollisions();

            foreach(Point pt in this.list_points)
            {
                pt.velocity = (pt.transform.position - pt.previous_position);
            }
        }

        private void DoubleDensityRelaxation()
        {
            foreach(Point pt in this.list_points)
            {
                float p = 0;
                float p_near = 0;

                foreach (Point pts in GetPointNeighbors(pt))
                {

                }
            }
        }

        private Hashtable GetPointNeighbors(Point pt)
        {
            Hashtable neighbors = new Hashtable();

            return neighbors;
        }
    }
}
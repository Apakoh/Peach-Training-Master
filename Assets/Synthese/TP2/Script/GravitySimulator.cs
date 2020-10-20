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

        [Range(0, 1)]
        public float base_density;

        [Range(1,20f)]
        public float radius_cohesion;

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
            GravitySimulation();
        }

        // GRAVITY SIMULATION

        private void GravitySimulation()
        {
            if (gravity_check)
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
            return Time.fixedDeltaTime;
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

                Hashtable neighbors = GetPointNeighbors(pt);

                foreach (Point neighbor in neighbors)
                {
                    float q = GetDistanceParticules(pt, neighbor) / this.radius_cohesion;
                    if(q < 1)
                    {
                        p += (1 - q) * (1 - q);
                    }
                }

                float pressure = this.k * (p - this.base_density);

                Vector3 dx = Vector3.zero;

                foreach (Point neighbor in neighbors)
                {
                    float q = GetDistanceParticules(pt, neighbor) / this.radius_cohesion;
                    if (q < 1)
                    {
                        Vector3 D = DeltaTime() * DeltaTime() * (p * (1 - q)) * (neighbor.transform.position - pt.transform.position).normalized;
                        neighbor.transform.position += D / 2;
                        dx -= D / 2;
                    }
                }

                pt.transform.position += dx;
            }
        }

        private float GetDistanceParticules(Point A, Point B)
        {
            return Vector3.Distance(A.transform.position, B.transform.position);
        }

        private Hashtable GetPointNeighbors(Point pt)
        {
            Hashtable neighbors = new Hashtable();

            foreach(Point neighbor in this.list_points)
            {
                if(GetDistanceParticules(pt, neighbor) < this.radius_cohesion)
                {
                    neighbors.Add(neighbor, neighbor);
                }
            }

            return neighbors;
        }
    }
}
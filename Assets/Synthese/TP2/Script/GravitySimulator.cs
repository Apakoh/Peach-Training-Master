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

        [Range(0, 10)]
        public float k;

        [Range(0, 10)]
        public float k_near;

        [Range(1, 10)]
        public float base_density;

        [Range(1,20f)]
        public float radius_cohesion;

        private void Start()
        {
            PointFactory pf = this.GetComponent<PointFactory>();
            this.list_points = pf.list_points;
            this.bundaries = pf.bundaries;

            Time.timeScale = 1f;
        }

        private void Update()
        {            
            GravitySimulation();
            ViscoSimulation();
            VelocityToPosition();
        }

        private void VelocityToPosition()
        {
            foreach(Point pt in this.list_points)
            {
                if (OutOfRange(pt.transform.position))
                {
                    float x = Mathf.Clamp(pt.transform.position.x, -this.bundaries, this.bundaries);
                    float y = Mathf.Clamp(pt.transform.position.y, -this.bundaries, this.bundaries);
                    float z = Mathf.Clamp(pt.transform.position.z, -this.bundaries, this.bundaries);
                    pt.transform.position = new Vector3(x, y, z);
                }                
            }
        }

        // GRAVITY SIMULATION

        private void GravitySimulation()
        {
            foreach (Point pt in this.list_points)
            {
                pt.velocity = Gravity(pt);
                pt.transform.position += pt.velocity;
            }            
        }

        private Vector3 Gravity(Point pt)
        {
           return DeltaTime() * pt.velocity + DeltaTime() * gravity;
        }

        private bool OutOfRange(Vector3 pos)
        {
            bool X = pos.x >= this.bundaries || pos.x <= -this.bundaries;
            bool Y = pos.y >= this.bundaries || pos.y <= -this.bundaries;
            bool Z = pos.z >= this.bundaries || pos.z <= -this.bundaries;

            return X || Y || Z;
        }

        private float DeltaTime()
        {
            return Time.fixedDeltaTime;
        }

        // VISCOELASTIC SIMULATION

        private void ViscoSimulation()
        {
            // ApplyViscosity();

            foreach(Point pt in this.list_points)
            {
                pt.previous_position = pt.transform.position;
                pt.transform.position += pt.velocity * DeltaTime();
            }

            // AdjustSprings();
            // ApplySpringDisplacements();
            DoubleDensityRelaxation();
            // ResolveCollisions();

            foreach(Point pt in this.list_points)
            {
                pt.velocity = (pt.transform.position - pt.previous_position) / DeltaTime();
            }
        }

        private void DoubleDensityRelaxation()
        {
            foreach(Point pt in this.list_points)
            {
                float p = 0;
                float p_near = 0;
                List<Point> neighbors = GetPointNeighbors(pt);

                foreach (Point neighbor in neighbors)
                {
                    float q = GetDistanceParticules(pt, neighbor) / this.radius_cohesion;

                    if(q < 1)
                    {
                        p += Mathf.Pow((1 - q), 2);
                        p_near += Mathf.Pow(1 - q, 3);
                    }
                }

                float pressure = this.k * (p - this.base_density);
                float pressure_near = this.k_near * p_near;

                Vector3 dx = Vector3.zero;

                foreach (Point neighbor in neighbors)
                {
                    float q = GetDistanceParticules(pt, neighbor) / this.radius_cohesion;
                    if (q < 1)
                    {
                        Vector3 D = Mathf.Pow(DeltaTime(), 2) * (pressure * (1 - q) + pressure_near * Mathf.Pow((1-q), 2)) * (neighbor.transform.position - pt.transform.position).normalized;
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

        private List<Point> GetPointNeighbors(Point pt)
        {
            List<Point> neighbors = new List<Point>();

            foreach(Point neighbor in this.list_points)
            {                
                if(GetDistanceParticules(pt, neighbor) <= this.radius_cohesion)
                {
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }

        private bool Grounded(Point pt)
        {
            return pt.transform.position.y <= -this.bundaries;
        }
    }
}
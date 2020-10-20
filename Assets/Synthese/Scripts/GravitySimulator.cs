using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthese
{
    public class GravitySimulator : MonoBehaviour
    {
        private List<Point> list_points = new List<Point>();

        private const float gravity = 9.8f;

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
            
        }

        private void Gravity()
        {

        }

        private void RandomVelocity(Point point)
        {
            point.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(RandomVector3(5f), RandomVector3(5f), RandomVector3(5f));
        }

        private float RandomVector3(float range)
        {
            return Random.Range(-range, range);
        }
    }
}
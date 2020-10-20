using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthese
{
    public class GravitySimulator : MonoBehaviour
    {
        private List<Point> list_points = new List<Point>();

        private void Start()
        {
            this.list_points = this.GetComponent<PointFactory>().list_points;
        }

        private void Update()
        {
            
        }

        private void Gravity()
        {

        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthese_TP2
{
    public class Point : MonoBehaviour
    {
        public Vector3 velocity;

        void Start()
        {
            this.velocity = Vector3.zero;
        }
    }
}
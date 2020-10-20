using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthese_TP1
{
    public class Constraint
    {
        public float angle_A;
        public float angle_B;

        public Constraint(float a, float b)
        {
            this.angle_A = a;
            this.angle_B = b;
        }
    }
}
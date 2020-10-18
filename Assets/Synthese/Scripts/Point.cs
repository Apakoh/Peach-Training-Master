using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthese
{
    public class Point : MonoBehaviour
    {
        public int index;

        public Vector3 old_position;

        public Synthese gm;

        public float bone_length;

        [Range(0f, 359f)]
        public float constraint_a = 0f;
        [Range(0f, 359f)]
        public float constraint_b = 90f;

        public Constraint constraint;

        void Start()
        {
            this.old_position = this.transform.position;
            this.constraint_a = 0f;
            this.constraint_b = 90f;
            this.constraint = new Constraint(constraint_a, constraint_b);
        }

        void Update()
        {
            this.constraint.angle_A = this.constraint_a;
            this.constraint.angle_B = this.constraint_b;

            if (this.old_position != this.transform.position)
            {
                this.old_position = this.transform.position;
                this.gm.list_points[index] = this.old_position;
                this.gm.RunArm();
            }
        }
    }
}
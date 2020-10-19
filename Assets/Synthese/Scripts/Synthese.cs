using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthese
{
    public class Synthese : MonoBehaviour
    {
        public List<Vector3> list_points = new List<Vector3>();

        public LineRenderer line_prefab;

        public GameObject point_prefab;

        public List<LineRenderer> list_lines = new List<LineRenderer>();
        public List<Point> list_points_go = new List<Point>();

        public GameObject target;

        private Vector3 old_target_position;

        private bool build = true;

        private void Start()
        {
            //this.old_target_position = this.target.transform.position;
            this.old_target_position = this.target.transform.position;
            InstantiatePoints();
        }

        private void Update()
        {
            this.target.transform.position = MousePosition();

            if (this.old_target_position != this.target.transform.position && build)
            {
                InverseKinetic();
                this.old_target_position = this.target.transform.position;
                StartCoroutine(TimerRebuild(0.002f));
            }
        }

        private void ResetLines()
        {
            foreach (LineRenderer line in list_lines)
            {
                Destroy(line.gameObject);
            }

            this.list_lines = new List<LineRenderer>();
        }

        private void ResetPoints()
        {
            foreach (Point point in list_points_go)
            {
                Destroy(point.gameObject);
            }

            this.list_points_go = new List<Point>();
        }

        private void UpdateListPoints()
        {
            for (int i = 0; i < this.list_points.Count; i++)
            {
                this.list_points[i] = this.list_points_go[i].transform.position;
            }
        }


        private void InstantiateLines()
        {
            ResetLines();

            for (int i = 0; i < this.list_points.Count - 1; i++)
            {
                Vector3 point_A = list_points[i];
                Vector3 point_B = list_points[i + 1];

                LineRenderer linetemp = Instantiate(line_prefab);

                linetemp.SetPosition(0, point_A);
                linetemp.SetPosition(1, point_B);

                this.list_lines.Add(linetemp);
            }
        }

        private void InstantiatePoints()
        {
            ResetPoints();

            for (int i = 0; i < this.list_points.Count; i++)
            {
                GameObject go = Instantiate(point_prefab, list_points[i], Quaternion.identity);
                Point up_pos = go.GetComponent<Point>();

                up_pos.gm = this;
                up_pos.index = i;

                if (i == 0)
                    up_pos.bone_length = 0;
                else
                    up_pos.bone_length = Vector3.Distance(this.list_points[i], this.list_points[i - 1]);

                this.list_points_go.Add(up_pos);
            }
        }

        public void RunArm()
        {
            UpdateListPoints();
            InstantiateLines();
        }

        public void InverseKinetic()
        {
            Forward();
            Backward();

            RunArm();
        }

        public void Forward()
        {
            this.list_points_go[this.list_points_go.Count - 1].transform.position = this.target.transform.position;

            for (int i = this.list_points.Count - 2; i > 0; i--)
            {
                Point A = this.list_points_go[i + 1];
                Point B = this.list_points_go[i];
                Point C = this.list_points_go[i - 1];

                Vector3 point_A = A.transform.position;
                Vector3 point_B = B.transform.position;
                Vector3 point_C = C.transform.position;

                float d1 = Vector3.Distance(point_B, point_A);
                float d2 = A.bone_length;

                if (!CompareDistance(d1, d2))
                {
                    Vector3 direction = (point_B - point_A).normalized;
                    B.transform.position = point_A + (direction * d2);
                }

                //AngleConstraintGestion(A, B, C);
            }
        }

        public void Backward()
        {
            for (int i = 1; i < this.list_points.Count; i++)
            {
                Point A = this.list_points_go[i - 1];
                Point B = this.list_points_go[i];

                Vector3 point_A = A.transform.position;
                Vector3 point_B = B.transform.position;

                float d1 = Vector3.Distance(point_B, point_A);
                float d2 = B.bone_length;

                Vector3 v1 = point_B - point_A;

                if (!CompareDistance(d1, d2))
                {
                    Vector3 direction = v1.normalized;
                    B.transform.position = point_A + (direction * d2);
                }

                if (i < this.list_points.Count - 1)
                {
                    Point C = this.list_points_go[i + 1];
                    Vector3 point_C = C.transform.position;
                    
                    //AngleConstraintGestion(A, B, C);
                }
            }
        }

        private void AngleConstraintGestion(Point A, Point B, Point C)
        {
            Vector3 point_A = A.transform.position;
            Vector3 point_B = B.transform.position;
            Vector3 point_C = C.transform.position;

            Vector3 x = (point_A - point_B).normalized;

            Vector3 y = Vector3.Cross(x, Vector3.forward);

            Vector3 direction_B_to_C = (point_C - point_B).normalized;

            float angle = Vector3.Angle(x, direction_B_to_C);

            if (angle > B.constraint.angle_B || angle < B.constraint.angle_A)
            {
                float radian = B.constraint.angle_B * Mathf.Deg2Rad;
                float coord_x = Mathf.Cos(radian);
                float coord_y = Mathf.Sin(radian);
                B.transform.position = new Vector3(coord_x, coord_y, 0) * B.bone_length;
            }
        }

        /*private float ClosestBorder(float angle_A, float angle_B)
        {
            float rest;
            if(angle_A - angle_B)
            return rest;
        }*/


        private bool CompareDistance(float d1, float d2)
        {
            return Math.Round(d1, 1) == Math.Round(d2, 1);
        }

        private void OnDrawGizmos()
        {
            if (this.list_points.Count == 0)
            {
                return;
            }

            for (int i = 0; i < this.list_points.Count; i++)
            {
                Gizmos.DrawIcon(this.list_points[i], i.ToString(), false);
            }
        }

        private IEnumerator TimerRebuild(float seconds)
        {
            this.build = false;
            yield return new WaitForSeconds(seconds);
            this.build = true;
        }

        private Vector3 MousePosition()
        {
            Vector3 mouse_position = Input.mousePosition;

            mouse_position.z = -1 * Camera.main.transform.position.z;

            return Camera.main.ScreenToWorldPoint(mouse_position);
        }

    }
}
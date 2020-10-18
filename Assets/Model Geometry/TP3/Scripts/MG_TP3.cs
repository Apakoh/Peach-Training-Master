using System.Collections.Generic;
using UnityEngine;

namespace MG_TP3
{
    public class MG_TP3 : MonoBehaviour
    {
        [Range(1, 15)]
        public int voxel_level;

        private int voxel_level_watch;

        [Range(1, 100)]
        public int size;

        [Range(1, 10)]
        public int radius;

        public Vector3[] list_spheres;

        public Vector3[] vertices_cube;
        public int[] triangles_cube;
        public int[] triangles_sphere;

        List<int> list_triangle = new List<int>();

        public Material mat;
        private MeshFilter meF;
        private MeshRenderer meR;

        private void Start()
        {
            this.meF = gameObject.AddComponent<MeshFilter>();
            this.meR = gameObject.AddComponent<MeshRenderer>();

            this.voxel_level_watch = this.voxel_level;

            Draw();
        }

        private void Update()
        {
            if (this.voxel_level != this.voxel_level_watch)
            {
                Draw();
                this.voxel_level_watch = this.voxel_level;
            }
        }

        private void CalculBox()
        {
            int distance, max = 0;
            foreach (Vector3 sphere_center in list_spheres)
            {
                distance = 2 + Mathf.FloorToInt(Vector3.Distance(Vector3.zero, sphere_center)) + this.radius;
                if (distance > max)
                {
                    max = distance;
                }
            }

            this.size = max;
        }

        private void Draw()
        {
            this.list_triangle = new List<int>();
            CalculBox();
            CreateVerticesCube();
            CreateTrianglesCube();
            //Voxel();
            Intersection();
            //Union();
            CreateTriangleSphereTab();
            CreateMesh();
        }

        private void Voxel()
        {
            float x, y, z;
            float condition;
            Vector3 cube;

            foreach (Vector3 sphere_center in list_spheres)
            {

                for (int i = 0; i < this.triangles_cube.Length - 36; i += 36)
                {
                    cube = CentralXYZ(i);
                    x = cube.x + sphere_center.x;
                    y = cube.y + sphere_center.y;
                    z = cube.z + sphere_center.z;

                    condition = x * x + y * y + z * z - this.radius;

                    if (condition < 0) //&& condition > -this.voxel_level/ this.voxel_level*2)
                    {
                        for (int j = 0; j < 36; j++)
                        {
                            list_triangle.Add(this.triangles_cube[i + j]);
                        }
                    }
                }
            }
        }

        private void Intersection()
        {
            float x, y, z;
            float eq_condition;
            bool draw_square;

            Vector3 cube;

            for (int i = 0; i < this.triangles_cube.Length - 36; i += 36)
            {
                draw_square = true;

                foreach (Vector3 sphere_center in list_spheres)
                {
                    cube = CentralXYZ(i);

                    x = cube.x + sphere_center.x;
                    y = cube.y + sphere_center.y;
                    z = cube.z + sphere_center.z;

                    eq_condition = x * x + y * y + z * z - this.radius;

                    if (eq_condition > 0)
                    {
                        draw_square = false;
                    }
                }

                if (draw_square)
                {
                    for (int j = 0; j < 36; j++)
                    {
                        list_triangle.Add(this.triangles_cube[i + j]);
                    }
                }
            }
        }

        private void Union()
        {
            float x, y, z;
            float eq_condition;

            bool draw_square;
            int nb_sphere_on_square;

            Vector3 cube;

            for (int i = 0; i < this.triangles_cube.Length - 36; i += 36)
            {
                draw_square = false;
                nb_sphere_on_square = 0;

                cube = CentralXYZ(i);

                foreach (Vector3 sphere_center in list_spheres)
                {
                    x = cube.x + sphere_center.x;
                    y = cube.y + sphere_center.y;
                    z = cube.z + sphere_center.z;

                    eq_condition = x * x + y * y + z * z - this.radius;

                    if (eq_condition < 0)
                    {
                        draw_square = true;
                        nb_sphere_on_square++;
                    }
                }

                //Debug.Log("Bool : " + draw_square + " Nb_Sphere : " + nb_sphere_on_square + " x,y,z : " + cube);
                if (draw_square && nb_sphere_on_square == 1)
                {
                    for (int j = 0; j < 36; j++)
                    {
                        list_triangle.Add(this.triangles_cube[i + j]);
                    }
                }
            }
        }

        private void CreateTriangleSphereTab()
        {
            this.triangles_sphere = new int[list_triangle.Count];

            for (int i = 0; i < list_triangle.Count; i++)
            {
                this.triangles_sphere[i] = list_triangle[i];
            }
        }

        private void CreateVerticesCube()
        {
            this.vertices_cube = new Vector3[(voxel_level * 2 + 1) * (voxel_level * 2 + 1) * (voxel_level * 2 + 1)];
            int index = 0;

            float coordinate_relative = (float)(this.size / 2) / this.voxel_level;


            for (int x = -this.voxel_level; x <= this.voxel_level; x++)
            {
                for (int y = -this.voxel_level; y <= this.voxel_level; y++)
                {
                    for (int z = -this.voxel_level; z <= this.voxel_level; z++)
                    {
                        this.vertices_cube[index] = new Vector3(coordinate_relative * x, coordinate_relative * y, coordinate_relative * z);
                        index++;
                    }
                }
            }
        }

        private void CreateTrianglesCube()
        {
            int nb_cube = this.voxel_level * 2 * this.voxel_level * 2 * this.voxel_level * 2;
            this.triangles_cube = new int[nb_cube * 6 * 6];

            int alpha = this.voxel_level * 2 + 1;
            int A, B, C, D, E, F, G, H;

            int index_triangles = 0;
            int cube;

            int step = this.voxel_level * 2 + 1;

            for (int i = 0; i < this.voxel_level * 2; i++)
            {
                for (int j = 0; j < this.voxel_level * 2; j++)
                {
                    for (int k = 0; k < this.voxel_level * 2; k++)
                    {
                        cube = (i * step) * step + j * step + k;

                        A = cube;
                        B = A + 1;
                        C = A + alpha;
                        D = C + 1;

                        E = cube + alpha * alpha;
                        F = E + 1;
                        G = E + alpha;
                        H = G + 1;

                        //Face 1 ABCD
                        this.triangles_cube[index_triangles] = A;
                        this.triangles_cube[index_triangles + 1] = B;
                        this.triangles_cube[index_triangles + 2] = C;
                        this.triangles_cube[index_triangles + 3] = C;
                        this.triangles_cube[index_triangles + 4] = B;
                        this.triangles_cube[index_triangles + 5] = D;

                        //Face 2 BFDH
                        this.triangles_cube[index_triangles + 6] = B;
                        this.triangles_cube[index_triangles + 7] = F;
                        this.triangles_cube[index_triangles + 8] = D;
                        this.triangles_cube[index_triangles + 9] = D;
                        this.triangles_cube[index_triangles + 10] = F;
                        this.triangles_cube[index_triangles + 11] = H;

                        //Face 3 FEHG
                        this.triangles_cube[index_triangles + 12] = F;
                        this.triangles_cube[index_triangles + 13] = E;
                        this.triangles_cube[index_triangles + 14] = H;
                        this.triangles_cube[index_triangles + 15] = H;
                        this.triangles_cube[index_triangles + 16] = E;
                        this.triangles_cube[index_triangles + 17] = G;

                        //Face 4 EAGC
                        this.triangles_cube[index_triangles + 18] = E;
                        this.triangles_cube[index_triangles + 19] = A;
                        this.triangles_cube[index_triangles + 20] = G;
                        this.triangles_cube[index_triangles + 21] = G;
                        this.triangles_cube[index_triangles + 22] = A;
                        this.triangles_cube[index_triangles + 23] = C;

                        //Face 5 ABEF
                        this.triangles_cube[index_triangles + 24] = B;
                        this.triangles_cube[index_triangles + 25] = A;
                        this.triangles_cube[index_triangles + 26] = E;
                        this.triangles_cube[index_triangles + 27] = B;
                        this.triangles_cube[index_triangles + 28] = E;
                        this.triangles_cube[index_triangles + 29] = F;

                        //Face 6 CDGH
                        this.triangles_cube[index_triangles + 30] = C;
                        this.triangles_cube[index_triangles + 31] = D;
                        this.triangles_cube[index_triangles + 32] = G;
                        this.triangles_cube[index_triangles + 33] = G;
                        this.triangles_cube[index_triangles + 34] = D;
                        this.triangles_cube[index_triangles + 35] = H;

                        index_triangles += 6 * 6;
                    }
                }
            }
        }

        private void CreateMesh()
        {
            Mesh msh = new Mesh();

            msh.vertices = this.vertices_cube;
            msh.triangles = this.triangles_sphere;

            this.meF.mesh = msh;
            this.meR.material = mat;
        }

        private Vector3 CentralXYZ(int index)
        {
            Vector3 central_point = Vector3.zero;

            int A, B, C, D, E, F, G, H;

            int alpha = this.voxel_level * 2 + 1;

            A = this.triangles_cube[index];
            B = A + 1;
            C = A + alpha;
            D = C + 1;

            E = A + alpha * alpha;
            F = E + 1;
            G = E + alpha;
            H = G + 1;

            central_point += this.vertices_cube[A];
            central_point += this.vertices_cube[B];
            central_point += this.vertices_cube[C];
            central_point += this.vertices_cube[D];
            central_point += this.vertices_cube[E];
            central_point += this.vertices_cube[F];
            central_point += this.vertices_cube[G];
            central_point += this.vertices_cube[H];

            return central_point / 8;
        }

        private void OnDrawGizmos()
        {
            if (this.vertices_cube == null)
            {
                return;
            }

            for (int i = 0; i < this.vertices_cube.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(transform.TransformPoint(this.vertices_cube[i]), 0.01f);
            }
        }
    }
}
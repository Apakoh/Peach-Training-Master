using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditorInternal;
using UnityEngine;

namespace MG_TP2
{
    public class MG_TP2 : MonoBehaviour
    {

        public string path_relative = "/Model Geometry/TP2/Materials/";

        public Maillage mesh = new Maillage();

        public Vector3[] vertices;
        Vector3[] normals;
        public int[] triangles;

        public Material mat;
        private MeshFilter meF;
        private MeshRenderer meR;

        public string file_name = "bunny.off";

        public string file_to_write;
        public bool write_file = false;

        private ReadFile read_file;

        private void Start()
        {
            this.meF = gameObject.AddComponent<MeshFilter>();
            this.meR = gameObject.AddComponent<MeshRenderer>();

            read_file = new ReadFile();
            string[] lines = read_file.ReadFileToString(file_name, path_relative);

            string[] line;
            int parse_int, index_second_part = 0;

            if (!(lines[0].Trim('\r', ' ') == "OFF")) { return; }

            for (int index = 2; index < lines.Length; index++)
            {
                line = lines[index].Split(char.Parse(" "));

                if (int.TryParse(line[0], out parse_int))
                {
                    if (parse_int == 3)
                    {
                        index_second_part = index;
                        break;
                    }
                }

                mesh.list_vertex.Add(new Vector3(read_file.ParseFloat(line[0]), read_file.ParseFloat(line[1]), read_file.ParseFloat(line[2])));
            }

            for (int index = index_second_part; index < lines.Length; index++)
            {
                line = lines[index].Split(char.Parse(" "));
                if (line.Length >= 3)
                {
                    int[] tab_temp = { read_file.ParseInt(line[1]), read_file.ParseInt(line[2]), read_file.ParseInt(line[3]) };
                    mesh.list_faces.Add(tab_temp);
                }
            }

            DrawMesh();
            CenterMesh();
            FixSize();
            CalculateNormals();

            if (this.write_file)
            {
                this.read_file.WriteFileOff(this.meF.mesh, file_to_write, path_relative);
            }
        }

        private void DrawMesh()
        {
            if (this.mesh.list_faces.Count == 0 || this.mesh.list_vertex.Count == 0)
            {
                return;
            }

            this.vertices = new Vector3[this.mesh.list_vertex.Count];

            for (int index = 0; index < this.vertices.Length; index++)
            {
                this.vertices[index] = this.mesh.list_vertex[index];
            }

            this.triangles = new int[(this.mesh.list_faces.Count) * 3];

            int index_bis;

            for (int index = 0; index < this.triangles.Length; index += 3)
            {
                index_bis = index / 3;
                this.triangles[index] = this.mesh.list_faces[index_bis][0];
                this.triangles[index + 1] = this.mesh.list_faces[index_bis][1];
                this.triangles[index + 2] = this.mesh.list_faces[index_bis][2];
            }

            Mesh msh = new Mesh();

            msh.vertices = vertices;
            msh.triangles = triangles;

            meF.mesh = msh;
            meR.material = mat;
        }

        private void CenterMesh()
        {
            float x = 0f, y = 0f, z = 0f;

            foreach (Vector3 vertice in this.mesh.list_vertex)
            {
                x += vertice.x;
                y += vertice.y;
                z += vertice.z;
            }

            x /= this.mesh.nb_vertex;
            y /= this.mesh.nb_vertex;
            z /= this.mesh.nb_vertex;

            Vector3 gravity_center = new Vector3(x, y, z);

            for (int index = 0; index < this.meF.mesh.vertexCount; index++)
            {
                this.meF.mesh.vertices[index] -= gravity_center;
            }
        }

        private void FixSize()
        {
            float max = 0;

            float x, y, z;

            foreach (Vector3 vertice in this.mesh.list_vertex)
            {
                x = Mathf.Abs(vertice.x);
                y = Mathf.Abs(vertice.y);
                z = Mathf.Abs(vertice.z);

                if (x > max)
                    max = x;
                if (y > max)
                    max = y;
                if (z > max)
                    max = z;
            }

            for (int index = 0; index < this.meF.mesh.vertexCount; index++)
            {
                this.meF.mesh.vertices[index] /= max;
            }
        }

        private void CalculateNormals()
        {
            this.normals = new Vector3[this.meF.mesh.vertexCount];

            for (int i = 0; i < this.normals.Length; i++)
            {
                this.normals[i] = Vector3.zero;
            }

            for (int i = 0; i < this.meF.mesh.triangles.Length; i += 3)
            {
                Vector3 a = this.meF.mesh.vertices[this.meF.mesh.triangles[i]];
                Vector3 b = this.meF.mesh.vertices[this.meF.mesh.triangles[i + 1]];
                Vector3 c = this.meF.mesh.vertices[this.meF.mesh.triangles[i + 2]];

                Vector3 normal = Vector3.Cross(b - a, c - a);

                this.normals[this.meF.mesh.triangles[i]] += normal;
                this.normals[this.meF.mesh.triangles[i + 1]] += normal;
                this.normals[this.meF.mesh.triangles[i + 2]] += normal;
            }

            for (int i = 0; i < this.normals.Length; i++)
            {
                this.normals[i] = this.normals[i].normalized;
            }

            this.meF.mesh.normals = this.normals;
        }
    }
}
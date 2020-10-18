using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MG_TP1 : MonoBehaviour
{
    public Material mat;
    private MeshFilter meF;
    private MeshRenderer meR;

    public bool plan, cylindre, sphere;

    // Plane
    public Vector3[] vertices_plane;
    public int[] triangles_plane;

    [Range(3, 20)]
    public int nb_cols;
    [Range(3, 20)]
    public int nb_lines;

    // Cylindre
    public Vector3[] vertices_cylindre;
    public int[] triangles_cylindre;

    [Range(3, 20)]
    public int cyl_radius;
    [Range(3, 20)]
    public int cyl_height;
    [Range(3, 100)]
    public int cyl_meridians;

    // Sphere
    public Vector3[] vertices_sphere;
    public int[] triangles_sphere;

    [Range(3, 20)]
    public int sph_radius;
    [Range(3, 50)]
    public int sph_meridians;
    public int sph_parallels;


    public Vector3[] normals;

    private void Start()
    {
        Create();
    }

    private void Update()
    {
        this.sph_parallels = this.sph_meridians - 1;
        DrawMesh();
    }

    private void Create()
    {
        this.meF = gameObject.AddComponent<MeshFilter>();
        this.meR = gameObject.AddComponent<MeshRenderer>();

        this.meR.material = this.mat;       
    }

    private void Reset()
    {
        this.vertices_plane = null;
        this.triangles_plane = null;

        this.vertices_cylindre = null;
        this.triangles_cylindre = null;

        this.vertices_sphere = null;
        this.triangles_sphere = null;
    }

    private void DrawMesh()
    {
        Mesh msh;

        Reset();

        if (plan)
        {
            // Plane Surface
            msh = Plan(this.nb_cols, this.nb_lines);
        }
        else if (cylindre)
        {
            // Cylindre
            msh = Cylindre(this.cyl_radius, this.cyl_height, this.cyl_meridians);
        }
        else
        {
            // Sphere
            msh = Sphere(this.sph_radius, this.sph_meridians, this.sph_parallels);
        }

        this.meF.mesh = msh;
    }

    private Mesh Plan(int nbC, int nbL)
    {
        int nbt = NumberTriangle(nbC, nbL);
        int nbver = (nbC + 1) * (nbL +1);
        int nbtri = 3 * nbt;

        vertices_plane = new Vector3[nbver];
        triangles_plane = new int[nbtri];

        CreationVerticesPlan(nbC, nbL, vertices_plane);
        CreationTrianglesPlan(nbC, nbL, triangles_plane);
        
        Mesh msh = new Mesh();

        msh.vertices = vertices_plane;
        msh.triangles = triangles_plane;

        return msh;  
    }

    private int NumberTriangle(int nbC, int nbL)
    {
        return nbC * nbL * 2;
    }

    private void CreationVerticesPlan(int nbL, int nbC, Vector3[] v)
    {
        int index;
        for(int i=0;i<=nbL; i++)
        {
            for(int j=0;j<=nbC; j++)
            {
                index = i * (nbC+1) + j;
                v[index] = new Vector3(j, i, 0);
            }    
        }
    }

    private void CreationTrianglesPlan(int nbL, int nbC, int[] t)
    {
        int index;
        int indexbis = 0;
        for(int i=0;i<nbL; i++)
        {
            for(int j=0;j<nbC; j++)
            {
                index = i * (nbC+1) + j;
                // Triangle 1
                t[indexbis + 1] = index + nbC + 2;
                t[indexbis] = index;
                t[indexbis + 2] = index + 1;  
                
                // Triangle 2
                t[indexbis + 5] = index;
                t[indexbis + 3] = index + nbC + 1;                
                t[indexbis+4] = index + nbC + 2;
                indexbis+=6;        
            }
        }
    }

    private void CreationTrianglesCylindre(int nbC, int[] t)
    {
        int index;
        int indexbis = 0;
        int A, B, C, D, E, F;

        // Top
        E = this.vertices_cylindre.Length - 1;
        // Bot
        F = this.vertices_cylindre.Length - 2;

        nbC--;

        for (int j = 0; j < nbC; j++)
        {
            index = j;

            A = index;
            B = A + 1;
            C = B + nbC;
            D = C + 1;

            // Triangle 1
            t[indexbis] = A;
            t[indexbis + 1] = B;
            t[indexbis + 2] = C;

            // Triangle 2
            t[indexbis + 3] = B;
            t[indexbis + 4] = D;
            t[indexbis + 5] = C;

            // Triangle Top
            t[indexbis + 6] = D;
            t[indexbis + 7] = E;
            t[indexbis + 8] = C;

            // Triangle Bot
            t[indexbis + 9] = A;
            t[indexbis + 10] = F;
            t[indexbis + 11] = B;

            indexbis += 12;
        }

        index = nbC;

        A = index;
        B = 0;
        D = nbC + 1;
        C = A + D;

        // Triangle 1
        t[indexbis] = A;
        t[indexbis + 1] = B;
        t[indexbis + 2] = C;

        // Triangle 2
        t[indexbis + 3] = B;
        t[indexbis + 4] = D;
        t[indexbis + 5] = C;

        // Triangle Top
        t[indexbis + 6] = D;
        t[indexbis + 7] = E;
        t[indexbis + 8] = C;

        // Triangle Bot
        t[indexbis + 9] = A;
        t[indexbis + 10] = F;
        t[indexbis + 11] = B;
    }

    private Mesh Cylindre(int radius, int height, int meridians)
    {
        float angle;
        float x, y, z, z_top;

        int index_top;

        this.vertices_cylindre = new Vector3[meridians*2+2];
        this.triangles_cylindre = new int[meridians*(12)];

        z = -height / 2;
        z_top = height / 2;

        for(int i=0; i < meridians; i++)
        {
            angle = (360 * i) / meridians;
            x = radius * Mathf.Cos((Mathf.PI / 180) * angle);
            y = radius * Mathf.Sin((Mathf.PI / 180) * angle);

            //Debug.Log(x * x + y * y);

            index_top = i + meridians;

            this.vertices_cylindre[i] = new Vector3(x, y, z);
            this.vertices_cylindre[index_top] = new Vector3(x, y, z_top);
        }

        // Setting North and South Vertice
        this.vertices_cylindre[this.vertices_cylindre.Length-2] = new Vector3(0, 0, z);
        this.vertices_cylindre[this.vertices_cylindre.Length-1] = new Vector3(0, 0, z_top);
        
        CreationTrianglesCylindre(meridians, this.triangles_cylindre);

        Mesh msh = new Mesh();
        msh.vertices = this.vertices_cylindre;
        msh.triangles = this.triangles_cylindre;

        return msh;
    }

    //private Mesh Sphere(float radius, float height, int meridians, int slices)
    //{
    //    float angle;
    //    float x, y, z, z_top, height_slice;

    //    float radius_slice;

    //    int index;

    //    this.vertices_sphere = new Vector3[(slices + 2) * meridians * 2];
    //    this.triangles_sphere = new int[(meridians*2) * 3 *(slices+1) * 2];

    //    height_slice = height / (slices + 1);

    //    for(int i=0; i < slices + 2; i++)
    //    {
    //        z = height_slice * (i + 1);
    //        z_top = height_slice * (i + 2);
    //        //radius_slice = radius * (0.8f - 0.1f *(i+1));
    //        radius_slice = radius - (radius / (i+1));

    //        for(int j=0; j < meridians; j++)
    //        {
    //            index = i * meridians + j;
    //            z = height_slice * (i + 1);
    //            z_top = height_slice * (i + 2);

    //            //Bot                
    //            angle = (360 * j) / meridians;
    //            x = radius_slice * Mathf.Cos((Mathf.PI / 180) * angle);
    //            y = radius_slice * Mathf.Sin((Mathf.PI / 180) * angle);
    //            this.vertices_sphere[index] = new Vector3(x, y, z);
    //            this.vertices_sphere[slices * meridians - 1 + index] = new Vector3(x, y, -z);      
    //        }
    //    }

    //    // this.vertices_cylindre[this.vertices_cylindre.Length-2] = new Vector3(0, 0, z);
    //    // this.vertices_cylindre[this.vertices_cylindre.Length-1] = new Vector3(0, 0, z_top);

    //    // North
    //    CreationTrianglesSphere(meridians, slices, this.triangles_sphere, 1);
    //    // South
    //    // CreationTrianglesSphere(meridians, slices, this.triangles_sphere, 2);

    //    Mesh msh = new Mesh();
    //    msh.vertices = this.vertices_sphere;
    //    msh.triangles = this.triangles_sphere;

    //    return msh;
    //}

    private void CreationTrianglesSphere(int meridians, int parallels, int[] t)
    {
        int index;
        int indexbis = 0;
        int A, B = 0, C, D = 0, E, F;

        // Top
        E = this.vertices_sphere.Length - 1;
        // Bot
        F = this.vertices_sphere.Length - 2;

        for (int i = 0; i < parallels - 2; i++)
        {
            for (int j = 0; j < meridians - 1; j++)
            {
                index = j + i * meridians;

                A = index;
                B = A + 1;
                C = A + meridians;
                D = C + 1;

                // Triangle 1
                t[indexbis] = A;
                t[indexbis + 1] = D;
                t[indexbis + 2] = B;

                // Triangle 2
                t[indexbis + 3] = A;
                t[indexbis + 4] = C;
                t[indexbis + 5] = D;

                indexbis += 6;
            }

            A = B;
            B = i * meridians;
            C = D;
            D = B + meridians;

            // Triangle 1
            t[indexbis] = A;
            t[indexbis + 1] = D;
            t[indexbis + 2] = B;

            // Triangle 2
            t[indexbis + 3] = A;
            t[indexbis + 4] = C;
            t[indexbis + 5] = D;

            indexbis += 6;
        }

        for(int k = 0; k < meridians - 1; k++)
        {
            // Triangle 1
            t[indexbis] = k;
            t[indexbis + 1] = k+1;
            t[indexbis + 2] = F;

            indexbis += 3;
        }

        // Triangle 1
        t[indexbis] = meridians - 1;
        t[indexbis + 1] = 0;
        t[indexbis + 2] = F;

        indexbis += 3;

        int start = this.vertices_sphere.Length - meridians - 2;

        for (int l = start; l < start + meridians - 1; l++)
        {
            // Triangle 1
            t[indexbis] = E;
            t[indexbis + 1] = l + 1;
            t[indexbis + 2] = l;

            indexbis += 3;
        }

        // Triangle 1
        t[indexbis] = start;
        t[indexbis + 1] = start + meridians - 1;
        t[indexbis + 2] = E;
    }

    private Mesh Sphere(float radius, int meridians, int parallels)
    {
        float x, y, z;
        // float south_pole, north_pole;

        int index;

        this.vertices_sphere = new Vector3[meridians * parallels + 2];
        int nb_triangle = (2 * meridians) * (parallels) + (meridians * 2);
        this.triangles_sphere = new int[nb_triangle * 3];

        parallels++;

        index = 0;

        for (int p = 1; p < parallels; p++)
        {
            for (int m = 0; m < meridians; m++)
            {
                // float x = Mathf.Sin(Mathf.PI * m / horizontalLines) * Mathf.Cos(2 * Mathf.PI * n / verticalLines);
                x = Mathf.Sin((Mathf.PI * p / parallels)) * Mathf.Cos((2 * Mathf.PI * m / meridians));
                //x = Mathf.RoundToInt(x);

                // float y = Mathf.Sin(Mathf.PI * m / horizontalLines) * Mathf.Sin(2 * Mathf.PI * n / verticalLines);
                y = Mathf.Sin((Mathf.PI * p / parallels)) * Mathf.Sin((2 * Mathf.PI * m / meridians));
                //y = Mathf.RoundToInt(y);

                // float z = Mathf.Cos(Mathf.PI * m / horizontalLines);
                z = Mathf.Cos((Mathf.PI * p / parallels));
                //z = Mathf.RoundToInt(z);

                //Debug.Log(x * x + y * y + z * z);

                this.vertices_sphere[index] = new Vector3(x, y, z) * radius;

                index++;
            }
        }

        // Setting North and South Vertice
        this.vertices_sphere[this.vertices_sphere.Length - 2] = new Vector3(0, 0, radius);
        this.vertices_sphere[this.vertices_sphere.Length - 1] = new Vector3(0, 0, -radius);

        CreationTrianglesSphere(meridians, meridians, this.triangles_sphere);

        Mesh msh = new Mesh();
        msh.vertices = this.vertices_sphere;
        msh.triangles = this.triangles_sphere;

        return msh;
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

    private void OnDrawGizmos()
    {
        if (this.vertices_sphere == null)
        {
            return;
        }

        for (int i = 0; i < this.vertices_sphere.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.TransformPoint(this.vertices_sphere[i]), 0.8f);
        }
    }
}

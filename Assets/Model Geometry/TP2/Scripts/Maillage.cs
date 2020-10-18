using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MG_TP2
{
    public class Maillage
    {
        // Sommets
        public int nb_vertex;
        public int nb_faces;
        // Arêtes
        public int nb_edges;

        public List<Vector3> list_vertex = new List<Vector3>();
        public List<int[]> list_faces = new List<int[]>();
    }
}
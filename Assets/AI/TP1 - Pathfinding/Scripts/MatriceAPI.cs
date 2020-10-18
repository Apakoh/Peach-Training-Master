using UnityEngine;

namespace AI_TP1
{
    public class MatriceAPI
    {
        public Vector3 MattoVect3(int i, int j, Node[,] matrice)
        {
            int x = matrice[i, j].GetX();
            int y = matrice[i, j].GetY();
            Vector3 vect = new Vector3(x, 0, y);
            return vect;
        }

        public Vector3 NodetoVect3(Node n)
        {
            int x = n.GetX();
            int y = n.GetY();
            Vector3 vect = new Vector3(x, 0, y);
            return vect;
        }
    }
}
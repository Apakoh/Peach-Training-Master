using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthese
{
    public class PointFactory : MonoBehaviour
    {
        [Range(0, 10000)]
        public int nb_point = 10000;

        public Point prefab_point;
        public GameObject parent_points;

        public List<Point> list_points = new List<Point>();

        void Start()
        {
            for(int i = 0; i < this.nb_point; i++)
            {
                Vector3 random_position_agent = GetRandomSpawnPoint(100, 100, 100, 2);
                Point new_point = Instantiate(this.prefab_point, random_position_agent, Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f)), this.parent_points.transform);
                new_point.name = "Point " + i;
                new_point.gameObject.GetComponentInChildren<SpriteRenderer>().color = RandomColor();
                list_points.Add(new_point);
            }
        }

        private Vector3 GetRandomSpawnPoint(float x, float y, float z, float radius)
        {
            Vector3 random_position_agent = new Vector3(Random.Range(-x, x), Random.Range(-y, y), Random.Range(-z, z));

            string[] list_tag = { "obstacles", "agent" };

            while (!PointSpawnAttempt(random_position_agent, radius, list_tag))
            {
                random_position_agent = new Vector3(Random.Range(-x, x), Random.Range(-y, y), Random.Range(-z, z));
            }

            return random_position_agent;
        }

        private bool PointSpawnAttempt(Vector3 position, float radius_to_check, string[] list_to_check)
        {
            Collider[] colliders = Physics.OverlapSphere(position, radius_to_check);
            bool valid_position = true;

            foreach (Collider col in colliders)
            {
                foreach (string element in list_to_check)
                {
                    if (col.tag == element)
                    {
                        valid_position = false;
                    }
                }
            }

            return valid_position;
        }

        private Color RandomColor()
        {
            return new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        }
    }
}
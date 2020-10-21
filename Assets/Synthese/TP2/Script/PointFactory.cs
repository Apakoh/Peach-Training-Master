using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Synthese_TP2
{
    public class PointFactory : MonoBehaviour
    {
        [Range(0, 10000)]
        public int nb_point = 10000;

        public Point prefab_point;
        public GameObject parent_points;

        public List<Point> list_points = new List<Point>();

        [Range(10, 1000)]
        public int bundaries = 100;

        void Start()
        {
            for(int i = 0; i < this.nb_point; i++)
            {
                Vector3 random_position_agent = GetRandomSpawnPoint(this.bundaries);
                Point new_point = Instantiate(this.prefab_point, random_position_agent, Quaternion.Euler(Vector3.zero), this.parent_points.transform);
                new_point.name = "Point " + i;
                new_point.gameObject.GetComponentInChildren<SpriteRenderer>().color = RandomColor();
                list_points.Add(new_point);
            }
        }

        private Vector3 GetRandomSpawnPoint(float bundaries)
        {
            Vector3 random_position_agent = new Vector3(Random.Range(-bundaries, bundaries), Random.Range(-bundaries, bundaries), Random.Range(-bundaries, bundaries));

            return random_position_agent;
        }

        private Color RandomColor()
        {
            return new Color(Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        }
    }
}
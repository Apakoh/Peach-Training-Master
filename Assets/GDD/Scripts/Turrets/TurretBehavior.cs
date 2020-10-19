using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class TurretBehavior : MonoBehaviour
    {
        public Turret stats;

        public GameObject projectile_prefab;

        public GameObject spawn;
        public List<GameObject> target_list;

        private float max_hp;
        private HPBar hp_bar;

        [Range(0.1f, 3f)]
        public float rate_of_fire = 1f;

        [Range(10f, 200f)]
        public float projectile_velocity = 100f;

        public bool can_fire = true;

        private void Start()
        {
            this.hp_bar = this.GetComponentInChildren<HPBar>();
            this.stats = Instantiate(stats);
            this.max_hp = this.stats.hp;
            this.target_list = new List<GameObject>();
        }

        public void TurretManagement()
        {
            if (this.stats.hp <= 0)
            {
                Destroy(this.gameObject);
            }

            if (this.target_list.Count > 0 && this.target_list[0] == null)
            {
                this.target_list.RemoveAt(0);
            }

            this.hp_bar.UpdateHPBar(this.max_hp, this.stats.hp);
        }

        public IEnumerator FireRateCD(float seconds)
        {
            this.can_fire = false;
            yield return new WaitForSeconds(seconds);
            this.can_fire = true;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class Projectile : MonoBehaviour
    {
        [Range(1f, 4f)]
        public float timer_alive = 2f;

        public TurretBehavior linked_turret;

        void Start()
        {
            Destroy(this.gameObject, this.timer_alive);
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.tag == "Enemy")
            {
                if(this.linked_turret.stats.type == "Damage")
                {
                    Destroy(this.gameObject);
                    TurretDamage turret = (TurretDamage)this.linked_turret.stats;
                    col.GetComponent<EnemyBehavior>().stats.hp -= turret.dps;
                }                
            }
        }
    }
}

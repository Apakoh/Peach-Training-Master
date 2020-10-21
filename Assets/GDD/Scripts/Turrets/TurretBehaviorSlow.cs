using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class TurretBehaviorSlow : TurretBehavior
    {
        private void Update()
        {
            TurretManagement();

            if (this.can_fire && target_list.Count > 0)
            {
                SlowTargets();
                StartCoroutine(FireRateCD(this.rate_of_fire));
            }
        }

        private void SlowTargets()
        {
            foreach(GameObject enemy in this.target_list)
            {
                EnemyBehavior enemy_b = enemy.GetComponent<EnemyBehavior>();
                if(!enemy_b.slowed)
                {
                    enemy_b.slowed = true;
                    enemy_b.slow_amout = ((TurretSlow)this.stats).slow_amount;
                }
                 
            }
        }
    }
}
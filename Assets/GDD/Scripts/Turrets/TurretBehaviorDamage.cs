using GDD;
using UnityEngine;

public class TurretBehaviorDamage : TurretBehavior
{
    private void Update()
    {
        TurretManagement();

        if (this.can_fire && target_list.Count > 0)
        {
            SpawnProjectile();
            StartCoroutine(FireRateCD(this.rate_of_fire));
        }
    }

    private void SpawnProjectile()
    {
        Vector3 position = spawn.transform.position;

        GameObject projectile = Instantiate(projectile_prefab, position, Quaternion.identity);
        projectile.GetComponent<Projectile>().linked_turret = this;
        if (projectile != null && target_list[0] != null)
        {
            projectile.GetComponent<Rigidbody>().velocity = (target_list[0].transform.position - projectile.transform.position).normalized * this.projectile_velocity;
        }
    }
}

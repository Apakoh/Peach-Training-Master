using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class RangeTrigger : MonoBehaviour
    {
        private TurretBehavior turret;

        private void Start()
        {
            this.turret = this.transform.parent.gameObject.GetComponent<TurretBehavior>();
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.tag == "Enemy")
            {
                this.turret.target_list.Add(col.gameObject);
            }
        }

        private void OnTriggerExit(Collider col)
        {
            this.turret.target_list.Remove(col.gameObject);
        }
    }
}

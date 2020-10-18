using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_TP2
{
    public class BulletManager : MonoBehaviour
    {
        public GameMasterTP2 gm;

        private void OnTriggerEnter(Collider col)
        {
            if (col.tag == "Agent")
            {
                Agent agent = col.GetComponent<Agent>();
                if (agent.can_be_killed)
                {
                    this.gm.agents.Remove(agent);
                    Destroy(col.transform.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
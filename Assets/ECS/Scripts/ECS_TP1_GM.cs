using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ECS
{
    public class ECS_TP1_GM : MonoBehaviour
    {
        public GameObject prefab_virus;

        public GameObject virus_parent;

        private bool can_spawn;

        private void Start()
        {
            this.can_spawn = true;
        }

        private void Update()
        {
            if (this.can_spawn)
            {
                Instantiate(prefab_virus, RandomSpawn(), Quaternion.identity);
                StartCoroutine(DelaySpawnVirus());
            }
        }

        private Vector3 RandomSpawn()
        {
            float x, y;

            x = Random.Range(-10f, 10f);
            y = Random.Range(-10f, 10f);

            return new Vector3(x, y, 0);
        }

        private IEnumerator DelaySpawnVirus()
        {
            this.can_spawn = false;
            yield return new WaitForSeconds(2);
            this.can_spawn = true;
        }
    }
}
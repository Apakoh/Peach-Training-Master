using System.Collections;
using UnityEngine;

namespace ECS
{
    public class ECS_TP1_IA : MonoBehaviour
    {
        public Vector3 target;

        public float speed = 2.5f;

        public bool moving;

        private void Start()
        {
            this.moving = false;
        }

        private void Update()
        {
            if (!this.moving)
            {
                StartCoroutine(DelayTarget());
            }

            MoveIA();
        }

        private void MoveIA()
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target, this.speed * Time.deltaTime);
        }

        private void RandomTarget()
        {
            float x, y;

            x = Random.Range(-10f, 10f);
            y = Random.Range(-10f, 10f);

            this.target = new Vector3(x, y, 0);
        }

        private IEnumerator DelayTarget()
        {
            RandomTarget();
            this.moving = true;
            yield return new WaitForSeconds(2);
            this.moving = false;
        }
    }
}
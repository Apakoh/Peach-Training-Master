using System.Collections;
using UnityEngine;

namespace ECS
{
    [DisallowMultipleComponent]
    public class ECS_TP1_Move : MonoBehaviour
    {
        public float speed = 2.5f;

        private void Update()
        {
            Vector3 movement = Vector3.zero;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                movement += Vector3.left;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                movement += Vector3.right;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                movement += Vector3.up;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                movement += Vector3.down;
            }

            this.transform.position += movement * this.speed * Time.deltaTime;
        }
    }
}
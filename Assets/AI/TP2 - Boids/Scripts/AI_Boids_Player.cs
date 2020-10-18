using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_TP2
{
    public class AI_Boids_Player : MonoBehaviour
    {
        [Range(1f, 15f)]
        public float speed_character = 5f;

        public bool hidden_mode = true;

        public bool alive = true;

        public GameMasterTP2 gm;

        public GameObject bullet_prefab;

        public GameObject spawn_bullet;

        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            this.hidden_mode = true;
        }

        private void Update()
        {
            ControlPlayer();
            SwitchStateControl();
            FireControl();
        }

        private void ControlPlayer()
        {
            Vector3 temp_vect = Vector3.zero;

            Vector3 rotation_axis = new Vector3(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z);

            if (Input.GetKey(KeyCode.Z))
            {
                temp_vect = this.transform.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                temp_vect = -this.transform.forward;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                temp_vect = -this.transform.right;
            }
            if (Input.GetKey(KeyCode.D))
            {
                temp_vect = this.transform.right;
            }

            temp_vect = temp_vect.normalized * 20 * Time.deltaTime + this.transform.position;
            this.Move(temp_vect);
        }

        private void SwitchStateControl()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                this.hidden_mode = !this.hidden_mode;
            }
        }

        private void FireControl()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Vector3 direction = Camera.main.ScreenToViewportPoint(new Vector3(0.5f, 0.5f, 0)).normalized;

                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                Vector3 direction = ray.GetPoint(120f);

                GameObject bullet_temp = Instantiate(this.bullet_prefab, this.spawn_bullet.transform.position, Quaternion.identity);
                bullet_temp.GetComponent<BulletManager>().gm = this.gm;
                bullet_prefab.transform.LookAt(direction);
                bullet_temp.GetComponent<Rigidbody>().AddForce(transform.forward * 50000f);
                Destroy(bullet_temp, 1f);
            }
        }

        private void Move(Vector3 pos_to_reach)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, pos_to_reach, this.speed_character * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.tag == "Agent")
            {
                this.alive = false;
            }
        }
    }
}
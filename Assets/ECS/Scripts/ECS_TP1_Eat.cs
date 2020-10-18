using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ECS
{
    public class ECS_TP1_Eat : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.tag == "Enemy")
            {
                Destroy(col.transform.gameObject);
            }
        }
    }
}
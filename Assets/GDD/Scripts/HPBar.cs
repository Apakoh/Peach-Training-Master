using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class HPBar : MonoBehaviour
    {
        private void Start()
        {
            this.GetComponent<Canvas>().worldCamera = Camera.main;
        }
        private void Update()
        {
            this.transform.forward = Camera.main.transform.forward;
        }
    }
}


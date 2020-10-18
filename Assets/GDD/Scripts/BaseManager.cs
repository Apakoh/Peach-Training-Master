using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class BaseManager : MonoBehaviour
    {
        public int hp;
        public int max_hp;

        private void Start()
        {
            this.hp = 20;
            this.max_hp = this.hp;
        }
    }
}

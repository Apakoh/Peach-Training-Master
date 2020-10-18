using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class GoldManager : MonoBehaviour
    {
        public int gold;

        private bool timer_gold = true;

        private float seconds_gold;

        private void Start()
        {
            this.gold = 0;
            this.seconds_gold = 2f;
        }

        private void Update()
        {
            if (this.timer_gold)
            {
                StartCoroutine(GoldPerSecond(this.seconds_gold));
            }
        }


        private IEnumerator GoldPerSecond(float seconds)
        {
            this.timer_gold = false;
            this.gold++;
            yield return new WaitForSeconds(seconds);
            this.timer_gold = true;
        }
    }
}
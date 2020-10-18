using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AI_TP2
{
    public class UIManager : MonoBehaviour
    {
        public float timer = 10f;

        public bool end = false;

        public TextMeshProUGUI timer_text;

        public Image game_over;
        public Image win;

        private void Update()
        {
            if (!this.end)
            {
                Timer();
            }
        }

        private void Timer()
        {
            timer -= Time.deltaTime;

            timer_text.text = ((int)timer).ToString();

            if (timer <= 0.0f)
            {
                this.end = true;
            }
        }
    }
}
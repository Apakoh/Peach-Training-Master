using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class HPBar : MonoBehaviour
    {
        private Slider hp_bar;

        private void Start()
        {
            this.hp_bar = this.GetComponentInChildren<Slider>();
            this.GetComponent<Canvas>().worldCamera = Camera.main;
        }
        private void Update()
        {
            this.transform.forward = Camera.main.transform.forward;
        }

        public void UpdateHPBar(float max_hp, float current_hp)
        {
            this.hp_bar.value = current_hp / max_hp;
        }
    }
}


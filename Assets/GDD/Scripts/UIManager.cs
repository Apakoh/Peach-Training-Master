using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GDD
{
    public class UIManager : MonoBehaviour
    {
        private WaveManager wm;
        private GoldManager gm;
        private BaseManager bm;

        public TextMeshProUGUI nb_wave_display;
        public TextMeshProUGUI nb_level_display;
        public TextMeshProUGUI nb_gold_display;
        public TextMeshProUGUI nb_base_display;

        public GameObject buy_menu;
        public GameObject end_game;

        public GameObject game_over_bg;
        public GameObject win_bg;

        public bool pause;

        void Start()
        {
            this.wm = this.GetComponent<WaveManager>();
            this.gm = this.GetComponent<GoldManager>();
            this.bm = this.GetComponent<BaseManager>();
            this.pause = false;
        }

        void Update()
        {
            CheckWin();
            CheckGameOver();
            this.nb_wave_display.text = (this.wm.current_wave_index + 1).ToString();
            this.nb_level_display.text = (this.wm.current_level_index + 1).ToString();
            this.nb_gold_display.text = (this.gm.gold).ToString();
            this.nb_base_display.text = (this.bm.hp).ToString();

            PauseGame();
        }

        private void CheckGameOver()
        {
            if(this.bm.hp <= 0)
            {
                this.end_game.SetActive(true);
                this.game_over_bg.SetActive(true);
                this.pause = true;
            }
        }

        private void CheckWin()
        {
            if (this.wm.end)
            {
                this.end_game.SetActive(true);
                this.win_bg.SetActive(true);
                this.pause = true;
            }
        }

        public void HideBuyMenu()
        {
            this.buy_menu.SetActive(false);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void PauseGame()
        {
            if (pause)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }
}

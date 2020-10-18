using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_TP2
{
    public class StateMachine : MonoBehaviour
    {
        public string current_state;

        private GameMasterTP2 gm;

        private UIManager ui_m;

        private AI_Boids_Player player;

        private void Start()
        {
            this.gm = this.GetComponent<GameMasterTP2>();
            this.ui_m = this.GetComponent<UIManager>();
            this.player = this.gm.player;
            this.current_state = "Hidden";
            StateHidden();
        }

        private void Update()
        {
            if (current_state != "Hidden" && this.player.hidden_mode)
            {
                this.current_state = "Hidden";
                StateHidden();
            }
            else if (current_state == "Hidden" && !this.player.hidden_mode)
            {
                this.current_state = "Chase";
                StateChase();
            }
            else if (this.ui_m.end || !this.player.alive)
            {
                StateGameOver();
            }
            else if (this.gm.agents.Count == 0)
            {
                StateWin();
            }
        }

        private void StateHidden()
        {
            foreach (Agent agent in this.gm.agents)
            {
                agent.can_be_killed = false;
            }

            // 5 => Weight of target behavior
            this.gm.weights[5] = 0;
        }

        private void StateChase()
        {
            foreach (Agent agent in this.gm.agents)
            {
                agent.can_be_killed = true;
            }
            // 5 => Weight of target behavior
            this.gm.weights[5] = 10;
        }

        private void StateGameOver()
        {
            ui_m.game_over.gameObject.SetActive(true);
        }

        private void StateWin()
        {
            ui_m.win.gameObject.SetActive(true);
        }
    }
}
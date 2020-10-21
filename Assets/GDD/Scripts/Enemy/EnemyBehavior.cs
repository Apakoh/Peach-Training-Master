using UnityEngine;
using PathCreation;
using UnityEngine.UI;
using System.Collections;

namespace GDD
{
    public class EnemyBehavior : MonoBehaviour
    {
        public Enemy stats;

        public PathCreator path_creator;
        public EndOfPathInstruction end_of_path_instruction;

        public GoldManager gold_m;
        public WaveManager wave_m;
        public BaseManager base_m;

        public float speed_ref;

        float distance_travelled;

        public Vector3 path_end;

        private HPBar hp_bar;
        private int max_hp;

        private bool attacking_turret;
        private TurretBehavior attacked_turret;
        private bool can_attack;
        private float cd_attack;

        public bool slowed;
        private bool slow_cd;
        public float slow_amout;

        void Start()
        {
            this.hp_bar = this.GetComponentInChildren<HPBar>();
            
            this.stats = Instantiate(stats);
            this.speed_ref = 10f;
            this.stats.speed *= this.speed_ref;
            this.speed_ref = this.stats.speed;
            this.max_hp = this.stats.hp;

            this.attacking_turret = false;
            this.can_attack = true;
            this.cd_attack = 1f;
            this.slowed = false;
            this.slow_cd = true;
            this.slow_amout = 0;

            if (path_creator != null)
            {
                path_creator.pathUpdated += OnPathChanged;
            }
        }

        void Update()
        {
            if (EndPath() || this.stats.hp <= 0)
            {
                Destroy(this.gameObject);
            }

            if(this.attacked_turret == null)
            {
                this.attacking_turret = false;
            }

            if (path_creator != null && !this.attacking_turret)
            {
                distance_travelled += this.stats.speed * Time.fixedDeltaTime;
                transform.position = path_creator.path.GetPointAtDistance(distance_travelled, end_of_path_instruction);
                transform.rotation = path_creator.path.GetRotationAtDistance(distance_travelled, end_of_path_instruction);
            }
            else if(this.attacking_turret && this.can_attack)
            {               
                this.attacked_turret.stats.hp -= this.stats.dps;
                StartCoroutine(CoolDownAttack(this.cd_attack));
            }

            this.hp_bar.UpdateHPBar(this.max_hp, this.stats.hp);

            if (this.slowed)
            {
                //Debug.Log(this.stats.speed);
            }
            if(this.slow_cd && this.slowed)
            {
                StartCoroutine(SlowCCActive());
            }
        }

        void OnPathChanged()
        {
            distance_travelled = path_creator.path.GetClosestDistanceAlongPath(transform.position);
        }

        private bool EndPath()
        {
            return this.transform.position == this.path_end;
        }

        private IEnumerator CoolDownAttack(float seconds)
        {
            this.can_attack = false;
            yield return new WaitForSeconds(seconds);
            this.can_attack = true;
        }

        private void OnDestroy()
        {
            this.wave_m.nb_spawned--;

            if (this.stats.hp <= 0)
            {
                this.gold_m.gold += this.stats.coins_given;
            }
            else
            {
                this.base_m.hp -= this.stats.dps;
            }
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.tag == "Turret")
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, col.gameObject.transform.position, this.stats.speed / this.speed_ref);
                this.attacking_turret = true;
                this.attacked_turret = col.gameObject.GetComponent<TurretBehavior>();
            }
        }

        private void OnTriggerExit(Collider col)
        {
            if (col.tag == "Turret")
            {
                this.attacking_turret = false;
                this.attacked_turret = null;
            }
        }

        private IEnumerator SlowCCActive()
        {
            this.slow_cd = false;
            this.stats.speed -= this.stats.speed * this.slow_amout;
            yield return new WaitForSeconds(1f);
            this.stats.speed = this.speed_ref;
            this.slow_amout = 0;
            this.slow_cd = true;
            this.slowed = false;
        }
    }
}
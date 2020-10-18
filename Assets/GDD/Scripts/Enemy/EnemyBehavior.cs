using UnityEngine;
using PathCreation;
using UnityEngine.UI;

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

        private Slider hp_bar;

        private int max_hp;

        void Start()
        {
            this.hp_bar = this.GetComponentInChildren<Slider>();
            
            this.stats = Instantiate(stats);
            this.speed_ref = 15f;
            this.stats.speed *= this.speed_ref;
            this.max_hp = this.stats.hp;

            this.hp_bar.maxValue = this.max_hp;

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

            if (path_creator != null)
            {
                distance_travelled += this.stats.speed * Time.deltaTime;
                transform.position = path_creator.path.GetPointAtDistance(distance_travelled, end_of_path_instruction);
                transform.rotation = path_creator.path.GetRotationAtDistance(distance_travelled, end_of_path_instruction);
            }

            this.hp_bar.value = this.stats.hp;
        }

        void OnPathChanged()
        {
            distance_travelled = path_creator.path.GetClosestDistanceAlongPath(transform.position);
        }

        private bool EndPath()
        {
            return this.transform.position == this.path_end;
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
    }
}
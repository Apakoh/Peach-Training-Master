using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    public class WaveManager : MonoBehaviour
    {
        public GameObject parasite_prefab;
        public GameObject infected_prefab;
        public GameObject coloss_prefab;

        public GameObject enemy_parent;

        public GoldManager gold_m;
        public BaseManager base_m;

        public List<Level> levels;

        private PathCreator path;

        public Vector3 spawn_location = new Vector3(-80, 2.5f, 5);
        public Vector3 end_location = new Vector3(55f, 2.5f, 55f);

        [Range(0.1f, 4f)]
        public float cd_spawn = 2f;
        private bool can_spawn = true;

        private bool next_wave;
        public bool end;

        public int nb_spawned;

        public Wave current_wave;

        public int current_level_index;
        public int current_wave_index;

        private void Start()
        {
            this.end = false;
            this.path = this.GetComponentInChildren<PathCreator>();
            this.gold_m = this.GetComponent<GoldManager>();
            this.base_m = this.GetComponent<BaseManager>();
            this.current_level_index = 0;
            this.next_wave = true;
            InitLevel();
        }

        private void Update()
        {
            CheckingStatus();
            GiveReward();
            SettingWaveToSpawn();
            SpawnWave();
        }

        private void InitLevel()
        {
            this.nb_spawned = 0;
            this.current_wave_index = 0;
            this.current_wave = Instantiate(this.levels[current_level_index].waves[current_wave_index]);
            this.next_wave = false;
        }

        private void ConfigWave()
        {
            this.base_m.hp = Mathf.Clamp(this.base_m.hp + 5, 0, 20);
            this.current_wave_index++;
            this.current_wave = Instantiate(this.levels[current_level_index].waves[current_wave_index]);
            this.next_wave = false;
        }

        private void SettingWaveToSpawn()
        {
            if (this.next_wave)
            {
                if (this.current_wave_index == this.levels[current_level_index].waves.Count - 1)
                {
                    if (this.current_level_index < this.levels.Count - 1)
                    {
                        InitLevel();
                        this.current_level_index++;
                        this.current_wave_index = 0;
                    }
                }

                if (this.current_wave_index < this.levels[current_level_index].waves.Count - 1)
                {
                    ConfigWave();
                }
            }
        }

        private bool CheckCurrentWaveEnded()
        {
            return this.current_wave.nb_parasite == 0 && this.current_wave.nb_infected == 0 && this.current_wave.nb_coloss == 0;
        }

        public void SpawnWave()
        {
            if (this.can_spawn)
            {
                if (this.current_wave.nb_parasite > 0 && RandomChoice())
                {
                    this.current_wave.nb_parasite--;
                    Spawn(path, parasite_prefab);
                }
                else if (this.current_wave.nb_infected > 0 && RandomChoice())
                {
                    this.current_wave.nb_infected--;
                    Spawn(path, infected_prefab);
                }
                else if (this.current_wave.nb_coloss > 0 && RandomChoice())
                {
                    this.current_wave.nb_coloss--;
                    Spawn(path, coloss_prefab);
                }
            }
        }

        private void Spawn(PathCreator path, GameObject unit_prefab)
        {
            SpawnUnit(path, unit_prefab);
            StartCoroutine(SpawnDelay(this.cd_spawn));
        }

        private void SpawnUnit(PathCreator path, GameObject unit_prefab)
        {
            GameObject enemy = Instantiate(unit_prefab, spawn_location, Quaternion.Euler(0, 90, 0), enemy_parent.transform);
            EnemyBehavior enemy_pf = enemy.GetComponent<EnemyBehavior>();

            enemy_pf.path_creator = path;
            enemy_pf.gold_m = this.gold_m;
            enemy_pf.wave_m = this;
            enemy_pf.base_m = this.base_m;
            enemy_pf.path_end = end_location;
            this.nb_spawned++;
        }

        private void CheckingStatus()
        {
            if (!this.end)
            {
                if (this.nb_spawned <= 0 && CheckCurrentWaveEnded())
                {
                    this.next_wave = true;

                    if (this.current_level_index == this.levels.Count - 1 && this.current_wave_index == this.levels[this.current_level_index].waves.Count - 1)
                    {
                        this.end = true;
                    }
                }
                else
                {
                    this.next_wave = false;
                }
            }
        }

        private void GiveReward()
        {
            if(this.next_wave && !this.end)
            {
                this.gold_m.gold += this.current_wave.reward;
            }            
        }

        private bool RandomChoice()
        {
            return Random.Range(0f, 10f) > 5f;
        }

        private IEnumerator SpawnDelay(float seconds)
        {
            this.can_spawn = false;
            yield return new WaitForSeconds(seconds);
            this.can_spawn = true;
        }
    }
}


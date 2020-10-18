using UnityEngine;

namespace ECS
{
	public class ECS_SpawnFactory : MonoBehaviour
	{
		// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
		public float spawn_time = 2f;
		public float spawn_cd = 0f;

		public int nb_max;
		public int nb_current;

		public GameObject prefab;
	}
}
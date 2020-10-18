using UnityEngine;
using FYFY;

namespace ECS
{
	public class ECS_VirusFactory : FSystem
	{
		private Family virus_factory = FamilyManager.getFamily(new AllOfComponents(typeof(ECS_SpawnFactory), typeof(ECS_Virus)));

		// Use this to update member variables when system pause. 
		// Advice: avoid to update your families inside this function.
		protected override void onPause(int currentFrame)
		{

		}

		// Use this to update member variables when system resume.
		// Advice: avoid to update your families inside this function.
		protected override void onResume(int currentFrame)
		{

		}

		// Use to process your families.
		protected override void onProcess(int familiesUpdateCount)
		{
			foreach (GameObject go in virus_factory)
			{
				ECS_SpawnFactory ecs_sf = go.GetComponent<ECS_SpawnFactory>();
				ecs_sf.spawn_cd += Time.deltaTime;

				if (ecs_sf.spawn_cd >= ecs_sf.spawn_time)
				{
					InstantiatePrefab(ecs_sf);
				}
			}
		}

		private Vector3 RandomSpawn()
		{
			float x, y;

			x = Random.Range(-7f, 7f);
			y = Random.Range(-7f, 7f);

			return new Vector3(x, y, 0);
		}

		private void InstantiatePrefab(ECS_SpawnFactory ecs_sf)
		{
			GameObject virus = Object.Instantiate(ecs_sf.prefab, RandomSpawn(), Quaternion.identity);
			GameObjectManager.bind(virus);
			ecs_sf.nb_current++;
			ecs_sf.spawn_cd = 0f;
		}

		public void PopVirus(int nb)
		{
			foreach (GameObject go in virus_factory)
			{
				ECS_SpawnFactory ecs_sf = go.GetComponent<ECS_SpawnFactory>();
				ecs_sf.nb_max = nb;

				while (ecs_sf.nb_current < ecs_sf.nb_max)
				{
					InstantiatePrefab(ecs_sf);
				}

				ecs_sf.nb_current = 0;
			}
		}
	}
}
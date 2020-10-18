using UnityEngine;
using FYFY;

namespace ECS
{
	public class ECS_BacterieFactory : FSystem
	{
		private Family bacteries = FamilyManager.getFamily(new AllOfComponents(typeof(ECS_SpawnFactory), typeof(ECS_Bacterie)));

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
			foreach (GameObject go in bacteries)
			{
				ECS_SpawnFactory ecs_sf = go.GetComponent<ECS_SpawnFactory>();
				if (ecs_sf.nb_current < 1)
				{
					InstantiatePrefab(ecs_sf);
				}
			}
		}

		private void InstantiatePrefab(ECS_SpawnFactory ecs_sf)
		{
			GameObject virus = Object.Instantiate(ecs_sf.prefab, RandomSpawn(), Quaternion.identity);
			GameObjectManager.bind(virus);
			ecs_sf.nb_current++;
			ecs_sf.spawn_cd = 0f;
		}

		private Vector3 RandomSpawn()
		{
			float x, y;

			x = Random.Range(-3f, 3f);
			y = Random.Range(-3f, 3f);

			return new Vector3(x, y, 0);
		}
	}
}
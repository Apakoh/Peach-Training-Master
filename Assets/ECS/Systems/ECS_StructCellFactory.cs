using UnityEngine;
using FYFY;

namespace ECS
{
	public class ECS_StructCellFactory : FSystem
	{
		private Family struct_cell_factory = FamilyManager.getFamily(new AllOfComponents(typeof(ECS_SpawnFactory), typeof(ECS_StructCell)));

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
			foreach (GameObject go in struct_cell_factory)
			{
				ECS_SpawnFactory ecs_sf = go.GetComponent<ECS_SpawnFactory>();
				ecs_sf.spawn_cd += Time.deltaTime;

				if (ecs_sf.spawn_cd >= ecs_sf.spawn_time)
				{
					GameObject virus = Object.Instantiate(ecs_sf.prefab, RandomSpawn(), Quaternion.identity);
					GameObjectManager.bind(virus);
					ecs_sf.spawn_cd = 0f;
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
	}
}
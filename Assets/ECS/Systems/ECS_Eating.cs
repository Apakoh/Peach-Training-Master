using UnityEngine;
using FYFY;
using FYFY_plugins.TriggerManager;

namespace ECS
{
	public class ECS_Eating : FSystem
	{
		private Family list_col = FamilyManager.getFamily(new AllOfComponents(typeof(Triggered2D)));

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
			foreach (GameObject go in list_col)
			{
				Triggered2D t = go.GetComponent<Triggered2D>();

				foreach (GameObject target in t.Targets)
				{
					if (target.tag == "Enemy")
					{
						try
						{
							GameObjectManager.unbind(target);
							Object.Destroy(target);
							Vector3 vec = go.transform.localScale;
							go.transform.localScale = new Vector3(vec.x + 0.01f, vec.y + 0.01f, vec.z + 0.01f);
						}
						catch (UnknownGameObjectException)
						{

						}
					}
				}
			}
		}
	}
}
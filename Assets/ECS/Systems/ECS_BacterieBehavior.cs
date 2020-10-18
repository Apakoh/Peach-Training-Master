using UnityEngine;
using FYFY;

namespace ECS
{
	public class ECS_BacterieBehavior : FSystem
	{
		private Family bacteries = FamilyManager.getFamily(new AllOfComponents(typeof(ECS_Bacterie)));


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

		}

	}
}
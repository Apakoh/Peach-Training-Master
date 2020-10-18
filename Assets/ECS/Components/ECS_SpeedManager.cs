using UnityEngine;

namespace ECS
{
	public class ECS_SpeedManager : MonoBehaviour
	{
		// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
		public float quick = 4;
		public float average = 2;
		public float slow = 1;
	}
}
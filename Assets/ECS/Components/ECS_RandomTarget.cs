using UnityEngine;

namespace ECS
{
	public class ECS_RandomTarget : MonoBehaviour
	{
		// Advice: FYFY component aims to contain only public members (according to Entity-Component-System paradigm).
		[HideInInspector]
		public Vector3 target;
	}
}
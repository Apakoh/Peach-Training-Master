using UnityEngine;
using FYFY;

[ExecuteInEditMode]
public class ECS_VirusFactory_wrapper : MonoBehaviour
{
	private void Start()
	{
		this.hideFlags = HideFlags.HideInInspector; // Hide this component in Inspector
	}

	public void PopVirus(System.Int32 nb)
	{
		MainLoop.callAppropriateSystemMethod ("ECS_VirusFactory", "PopVirus", nb);
	}

}
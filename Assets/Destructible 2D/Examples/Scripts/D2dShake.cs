using UnityEngine;

namespace Destructible2D
{
	// This component automatically adds shake to the D2dCameraShake component
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Shake")]
	public class D2dShake : MonoBehaviour
	{
		public float Shake;
		
		protected virtual void Awake()
		{
			D2dCameraShake.Shake += Shake;
		}
	}
}
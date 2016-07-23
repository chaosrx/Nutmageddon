using UnityEngine;

namespace Destructible2D
{
	// This component spawns a prefab under the mouse when you click
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Click To Spawn")]
	public class D2dClickToSpawn : MonoBehaviour
	{
		[Tooltip("The key you must hold down to spawn")]
		public KeyCode Requires = KeyCode.Mouse0;
		
		[Tooltip("The prefab that gets spawned under the mouse when clicking")]
		public GameObject Prefab;
		
		// The cached main camera
		private Camera mainCamera;
		
		protected virtual void Update()
		{
			// Get the main camera?
			if (mainCamera == null) mainCamera = Camera.main;
			
			if (Input.GetKeyDown(Requires) == true)
			{
				if (Prefab != null && mainCamera != null)
				{
					var screenPoint   = Input.mousePosition;
					var worldPoint    = Camera.main.ScreenToWorldPoint(screenPoint);
					var worldRotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)); // Random rotation around Z axis
					
					Instantiate(Prefab, worldPoint, worldRotation);
				}
			}
		}
	}
}
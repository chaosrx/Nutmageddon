using UnityEngine;

namespace Destructible2D
{
	// This component fractures the Destructible under the mouse and then spawns a prefab there
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Click To Explode")]
	public class D2dClickToExplode : MonoBehaviour
	{
		[Tooltip("The key you must hold down to spawn")]
		public KeyCode Requires = KeyCode.Mouse0;
		
		[Tooltip("The prefab that gets spawned under the mouse when clicking")]
		public GameObject ExplosionPrefab;
		
		[Tooltip("The amount of times you want the clicked object to fracture")]
		public int FractureCount = 5;
		
		// The cached main camera
		private Camera mainCamera;
		
		protected virtual void Update()
		{
			// Get the main camera?
			if (mainCamera == null) mainCamera = Camera.main;
			
			if (Input.GetKeyDown(Requires) == true)
			{
				if (mainCamera != null)
				{
					var screenPoint   = Input.mousePosition;
					var worldPoint    = Camera.main.ScreenToWorldPoint(screenPoint);
					var collider      = Physics2D.OverlapPoint(worldPoint);
					
					if (collider != null)
					{
						var destructible = collider.GetComponentInParent<D2dDestructible>();
						
						if (destructible != null)
						{
							D2dQuadFracturer.Fracture(destructible, FractureCount, 0.5f, 1);
							
							if (ExplosionPrefab != null)
							{
								var worldRotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)); // Random rotation around Z axis
								
								Instantiate(ExplosionPrefab, worldPoint, worldRotation);
							}
						}
					}
				}
			}
		}
	}
}
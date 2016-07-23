using UnityEngine;

namespace Destructible2D
{
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Drag To Throw")]
	public class D2dDragToThrow : MonoBehaviour
	{
		[Tooltip("The key you must hold down to do slicing")]
		public KeyCode Requires = KeyCode.Mouse0;
		
		[Tooltip("The prefab used to show what the slice will look like")]
		public GameObject IndicatorPrefab;
		
		[Tooltip("The scale of the throw indicator")]
		public float Scale = 1.0f;
		
		[Tooltip("The prefab that gets thrown")]
		public GameObject ProjectilePrefab;
		
		[Tooltip("How fast the projectile will be launched")]
		public float ProjectileSpeed;
		
		[Tooltip("How much spread is added to the project when fired")]
		public float ProjectileSpread;
		
		// Currently slicing?
		private bool down;
		
		// Mouse position when slicing began
		private Vector3 startMousePosition;
		
		// Instance of the indicator
		private GameObject indicatorInstance;
		
		// The cached main camera
		private Camera mainCamera;
		
		protected virtual void Update()
		{
			// Get the main camera?
			if (mainCamera == null) mainCamera = Camera.main;
			
			// Begin dragging
			if (Input.GetKey(Requires) == true && down == false)
			{
				down               = true;
				startMousePosition = Input.mousePosition;
			}
			
			// End dragging
			if (Input.GetKey(Requires) == false && down == true)
			{
				down = false;
				
				// Throw prefab?
				if (mainCamera != null && ProjectilePrefab != null)
				{
					var projectile  = Instantiate(ProjectilePrefab);
					var startPos    = mainCamera.ScreenToWorldPoint( startMousePosition);
					var currentPos  = mainCamera.ScreenToWorldPoint(Input.mousePosition);
					var angle       = D2dHelper.Atan2(currentPos - startPos) * Mathf.Rad2Deg;
					var rigidbody2D = projectile.GetComponent<Rigidbody2D>();
					
					if (rigidbody2D != null)
					{
						rigidbody2D.velocity = (currentPos - startPos) * ProjectileSpeed;
					}
					
					angle += Random.Range(-ProjectileSpread, ProjectileSpread);
					
					projectile.transform.position = startPos;
					projectile.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -angle);
				}
			}
			
			// Update indicator?
			if (down == true && mainCamera != null && IndicatorPrefab != null)
			{
				if (indicatorInstance == null)
				{
					indicatorInstance = Instantiate(IndicatorPrefab);
				}
				
				var startPos   = mainCamera.ScreenToWorldPoint( startMousePosition);
				var currentPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
				var scale      = Vector3.Distance(currentPos, startPos) * Scale;
				var angle      = D2dHelper.Atan2(currentPos - startPos) * Mathf.Rad2Deg;
				
				// Transform the indicator so it lines up with the slice
				indicatorInstance.transform.position   = new Vector3(startPos.x, startPos.y, indicatorInstance.transform.position.z);
				indicatorInstance.transform.rotation   = Quaternion.Euler(0.0f, 0.0f, -angle);
				indicatorInstance.transform.localScale = new Vector3(scale, scale, scale);
			}
			// Destroy indicator?
			else if (indicatorInstance != null)
			{
				Destroy(indicatorInstance.gameObject);
			}
		}
	}
}
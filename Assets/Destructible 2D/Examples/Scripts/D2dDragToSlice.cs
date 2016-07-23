using UnityEngine;

namespace Destructible2D
{
	// This component allows you to slice all Destructibles under the mouse slice
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Drag To Slice")]
	public class D2dDragToSlice : MonoBehaviour
	{
		[Tooltip("The key you must hold down to do slicing")]
		public KeyCode Requires = KeyCode.Mouse0;
		
		[Tooltip("The prefab used to show what the slice will look like")]
		public GameObject IndicatorPrefab;
		
		[Tooltip("The shape of the slice when it stamps the Destructibles in the scene")]
		public Texture2D StampTex;
		
		[Tooltip("How hard the stamp should be")]
		public float Hardness = 1.0f;
		
		[Tooltip("The thickness of the slice line")]
		public float Thickness = 1.0f;
		
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
				
				// Slice all Destructibles?
				if (mainCamera != null)
				{
					var endMousePosition = Input.mousePosition;
					var startPos         = mainCamera.ScreenToWorldPoint(startMousePosition);
					var endPos           = mainCamera.ScreenToWorldPoint(  endMousePosition);
					
					D2dDestructible.SliceAll(startPos, endPos, Thickness, StampTex, Hardness);
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
				var scale      = Vector3.Distance(currentPos, startPos);
				var angle      = D2dHelper.Atan2(currentPos - startPos) * Mathf.Rad2Deg;
				
				// Transform the indicator so it lines up with the slice
				indicatorInstance.transform.position   = new Vector3(startPos.x, startPos.y, indicatorInstance.transform.position.z);
				indicatorInstance.transform.rotation   = Quaternion.Euler(0.0f, 0.0f, -angle);
				indicatorInstance.transform.localScale = new Vector3(Thickness, scale, scale);
			}
			// Destroy indicator?
			else if (indicatorInstance != null)
			{
				Destroy(indicatorInstance.gameObject);
			}
		}
	}
}
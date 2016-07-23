using UnityEngine;

namespace Destructible2D
{
	// This component causes the current GameObject to follow the main camera on the x/y axis
	[ExecuteInEditMode]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Tile")]
	public class D2dTile : MonoBehaviour
	{
		public Vector2 Size;
		
		public D2dVector2 Offset;
		
		private Camera mainCamera;
		
		private Renderer mainRenderer;
		
		public void UpdatePosition(Vector2 offset)
		{
			if (mainCamera == null) mainCamera = Camera.main;
			
			if (mainCamera != null && Size.x > 0.0f && Size.y > 0.0f)
			{
				var position = transform.localPosition;
				var observer = mainCamera.transform.position - (Vector3)offset;
				
				position.x = Mathf.RoundToInt(observer.x / Size.x + Offset.X) * Size.x + offset.x;
				position.y = Mathf.RoundToInt(observer.y / Size.y + Offset.Y) * Size.y + offset.y;
				
				transform.localPosition = position;
			}
		}
		
		public void UpdateRenderer(int sortingOrder)
		{
			if (mainRenderer == null) mainRenderer = GetComponent<Renderer>();
			
			if (mainRenderer != null)
			{
				mainRenderer.sortingOrder = sortingOrder;
			}
		}
		
		protected virtual void Update()
		{
			UpdatePosition(Vector3.zero);
		}
	}
}
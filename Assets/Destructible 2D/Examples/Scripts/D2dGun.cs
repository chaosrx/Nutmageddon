using UnityEngine;

namespace Destructible2D
{
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Gun")]
	public class D2dGun : MonoBehaviour
	{
		public float MoveScale = 0.25f;
		
		public float MoveSpeed = 5.0f;
		
		public GameObject MuzzlePrefab;
		
		public GameObject BulletPrefab;
		
		protected virtual void Update()
		{
			var localPosition = transform.localPosition;
			var targetX       = (Input.mousePosition.x - Screen.width  / 2) * MoveScale;
			var targetY       = (Input.mousePosition.y - Screen.height / 2) * MoveScale;
			
			localPosition.x = D2dHelper.Dampen(localPosition.x, targetX, MoveSpeed, Time.deltaTime);
			localPosition.y = D2dHelper.Dampen(localPosition.y, targetY, MoveSpeed, Time.deltaTime);
			
			transform.localPosition = localPosition;
			
			// Left click?
			if (Input.GetMouseButtonDown(0) == true)
			{
				var mainCamera = Camera.main;
				
				if (MuzzlePrefab != null)
				{
					Instantiate(MuzzlePrefab, transform.position, Quaternion.identity);
				}
				
				if (BulletPrefab != null && mainCamera != null)
				{
					var position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
					
					Instantiate(BulletPrefab, position, Quaternion.identity);
				}
			}
		}
	}
}
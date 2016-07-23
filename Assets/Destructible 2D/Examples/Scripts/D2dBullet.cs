using UnityEngine;
using System.Collections;

namespace Destructible2D
{
	[ExecuteInEditMode]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Bullet")]
	public class D2dBullet : MonoBehaviour
	{
		public string IgnoreTag;
		
		public LayerMask RaycastMask = -1;
		
		public GameObject ExplosionPrefab;
		
		public float Speed;
		
		public float MaxLength;
		
		public Vector3 MaxScale;
		
		private Vector3 oldPosition;
		
		protected virtual void Start()
		{
			oldPosition = transform.position;
		}
		
		protected virtual void FixedUpdate()
		{
			var newPosition  = transform.position;
			var rayLength    = (newPosition - oldPosition).magnitude;
			var rayDirection = (newPosition - oldPosition).normalized;
			var hit          = Physics2D.Raycast(oldPosition, rayDirection, rayLength, RaycastMask);
			
			// Update old position to trail behind 
			if (rayLength > MaxLength)
			{
				rayLength   = MaxLength;
				oldPosition = newPosition - rayDirection * rayLength;
			}
			
			transform.localScale = MaxScale * D2dHelper.Divide(rayLength, MaxLength);
			
			if (hit.collider != null)
			{
				if (string.IsNullOrEmpty(IgnoreTag) == true || hit.collider.tag != IgnoreTag)
				{
					if (ExplosionPrefab != null)
					{
						Instantiate(ExplosionPrefab, hit.point, Quaternion.identity);
					}
					
					Destroy(gameObject);
				}
			}
		}
		
		protected virtual void Update()
		{
			transform.Translate(0.0f, Speed * Time.deltaTime, 0.0f);
		}
		
#if UNITY_EDITOR
		protected virtual void OnDrawGizmos()
		{
			Gizmos.DrawLine(transform.position, transform.TransformPoint(0.0f, -MaxLength, 0.0f));
		}
#endif
	}
}
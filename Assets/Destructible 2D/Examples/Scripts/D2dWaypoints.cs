using UnityEngine;

namespace Destructible2D
{
	// This component automatically moves the current GameObject between waypoints
	[RequireComponent(typeof(Rigidbody2D))]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Waypoints")]
	public class D2dWaypoints : MonoBehaviour
	{
		public Vector2[] Points;
		
		public float Acceleration = 5.0f;

		public float MaximumSpeed = 2.0f;

		public float SpeedBoost = 2.0f;

		public float MinimumDistance = 1.0f;

		private Vector2 targetPoint;
		
		private Rigidbody2D mainRigidbody;

		protected virtual void Awake()
		{
			ChangeTargetPoint();
        }

		protected virtual void FixedUpdate()
		{
			if (mainRigidbody == null) mainRigidbody = GetComponent<Rigidbody2D>();
			
			var currentPoint = (Vector2)transform.position;
			var vector       = targetPoint - currentPoint;

			if (vector.magnitude <= MinimumDistance)
			{
				ChangeTargetPoint();

				vector = targetPoint - currentPoint;
			}

			// Limit target speed
			if (vector.magnitude > MaximumSpeed)
			{
				vector = vector.normalized * MaximumSpeed;
			}
			
			// Acceleration
			mainRigidbody.velocity = D2dHelper.Dampen2(mainRigidbody.velocity, vector * SpeedBoost, Acceleration, Time.deltaTime);
		}

		private void ChangeTargetPoint()
		{
			var newIndex = Random.Range(0, Points.Length);

			targetPoint = Points[newIndex];
		}

#if UNITY_EDITOR
		protected virtual void OnDrawGizmosSelected()
		{
			if (Points != null)
			{
				foreach (var point in Points)
				{
					Gizmos.DrawLine(point, transform.position);
				}
			}
		}
#endif
	}
}
using UnityEngine;

namespace Destructible2D
{
	// This component causes the current GameObject to follow the target Transform
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Player Camera")]
	public class D2dPlayerCamera : MonoBehaviour
	{
		public float Acceleration = 1.0f;

		public float Dampening = 2.0f;

		private Vector2 velocity;

		protected virtual void Update()
		{
			var h = Input.GetAxisRaw("Horizontal");
			var v = Input.GetAxisRaw("Vertical");

			velocity.x += h * Acceleration * Time.deltaTime;
			velocity.y += v * Acceleration * Time.deltaTime;

			velocity = D2dHelper.Dampen2(velocity, Vector2.zero, Dampening, Time.deltaTime);

			transform.Translate(velocity);
		}
	}
}

using UnityEngine;

namespace Destructible2D
{
	// This component causes the current GameObject to detach from its parent and adds a Rigidbody2D
	[DisallowMultipleComponent]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Compound Part")]
	public class D2dCompoundPart : MonoBehaviour
	{
		public float MaxForce = 1.0f;

		[SerializeField]
		private Rigidbody2D thisRigidbody;

		[SerializeField]
		private Vector2 acceleration;

		public void OnDetach()
		{
			// Detach
			transform.parent = null;

			// Add rigidbody
			thisRigidbody = gameObject.AddComponent<Rigidbody2D>();

			thisRigidbody.gravityScale = 0.0f;

			acceleration = Random.insideUnitCircle * MaxForce;
		}

		protected virtual void FixedUpdate()
		{
			if (thisRigidbody != null)
			{
				thisRigidbody.AddRelativeForce(acceleration, ForceMode2D.Force);
			}
		}
	}
}

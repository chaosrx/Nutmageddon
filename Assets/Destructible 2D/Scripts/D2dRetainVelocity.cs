using UnityEngine;
using System.Collections.Generic;

namespace Destructible2D
{
	// This component causes the Rigidbody2D velocity to carry over after a Destructible has been split
	[DisallowMultipleComponent]
	[RequireComponent(typeof(D2dDestructible))]
	[RequireComponent(typeof(Rigidbody2D))]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Retain Velocity")]
	public class D2dRetainVelocity : MonoBehaviour
	{
		private Rigidbody2D mainRigidbody2D;

		private D2dDestructible destructible;

		private float angularVelocity;

		private Vector2 velocity;

		protected virtual void OnEnable()
		{
			if (destructible              == null) destructible              = GetComponent<D2dDestructible>();
			if (destructible.OnStartSplit == null) destructible.OnStartSplit = new D2dEvent();
			if (destructible.OnEndSplit   == null) destructible.OnEndSplit   = new D2dDestructibleListEvent();

			destructible.OnStartSplit.AddListener(OnStartSplit);
			destructible.OnEndSplit  .AddListener(OnEndSplit);
		}

		protected virtual void OnDisable()
		{
			destructible.OnStartSplit.RemoveListener(OnStartSplit);
			destructible.OnEndSplit  .RemoveListener(OnEndSplit);
		}

		protected virtual void OnStartSplit()
		{
			if (mainRigidbody2D == null) mainRigidbody2D = GetComponent<Rigidbody2D>();

			velocity        = mainRigidbody2D.velocity;
			angularVelocity = mainRigidbody2D.angularVelocity;
		}

		protected virtual void OnEndSplit(List<D2dDestructible> clones)
		{
			for (var i = clones.Count - 1; i >= 0; i--)
			{
				var clone = clones[i];

				if (clone.gameObject != gameObject)
				{
					var cloneRigidbody2D = clone.GetComponent<Rigidbody2D>();

					if (cloneRigidbody2D != null)
					{
						cloneRigidbody2D.velocity        = velocity;
						cloneRigidbody2D.angularVelocity = angularVelocity;
					}
				}
			}
		}
	}
}

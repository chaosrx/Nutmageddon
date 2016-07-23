using UnityEngine;
using System.Collections;

namespace Destructible2D
{
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Collision Handler")]
	public class D2dCollisionHandler : MonoBehaviour
	{
		[Tooltip("Output debug information about collisions?")]
		public bool DebugCollisions;
		
		[Tooltip("The layers that must hit this collider for damage to get inflicted")]
		public LayerMask ImpactMask = -1;

		[Tooltip("This amount of damage required to register an impact")]
		public float ImpactThreshold = 1.0f;
		
		[Tooltip("The minimum amount of seconds between each impact")]
		public float ImpactDelay;

		[Tooltip("Should this collider inflict damage on the Destructible when it takes impact?")]
		public bool DamageOnImpact;
		
		[Tooltip("The destructible that takes the damage")]
		public D2dDestructible DamageDestructible;

		[Tooltip("The damage will be scaled by this value after it passes the ImpactThreshold")]
		public float DamageScale = 1.0f;
		
		public D2dVector2Event OnImpact;

		private float cooldownTime;

		protected virtual void OnCollisionEnter2D(Collision2D collision)
		{
			if (DebugCollisions == true)
			{
				Debug.Log(name + " hit " + collision.collider.name + " for " + collision.relativeVelocity.magnitude);
			}

			if (ImpactDelay > 0.0f)
			{
				if (Time.time >= cooldownTime)
				{
					cooldownTime = Time.time + ImpactDelay;
				}
				else
				{
					return;
				}
			}

			var collisionMask = 1 << collision.collider.gameObject.layer;

			if ((collisionMask & ImpactMask) != 0)
			{
				var contacts = collision.contacts;

				for (var i = contacts.Length - 1; i >= 0; i--)
				{
					var contact = contacts[i];
					var impact  = collision.relativeVelocity.magnitude;

					if (impact >= ImpactThreshold)
					{
						if (DamageOnImpact == true)
						{
							if (DamageDestructible == null) DamageDestructible = GetComponentInChildren<D2dDestructible>();

							if (DamageDestructible != null)
							{
								DamageDestructible.Damage += impact * DamageScale;
							}
						}

						if (OnImpact != null) OnImpact.Invoke(contact.point);
					}
				}
			}
		}
	}
}
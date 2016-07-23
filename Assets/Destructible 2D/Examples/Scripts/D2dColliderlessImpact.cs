using UnityEngine;
using System.Collections;

namespace Destructible2D
{
	// This component will raycast all destructibles between the previous and current position of this GameObject
	// If a solid pixel is found, then this GameObject will be destroyed, and the impact prefab will be spawned in its place
	[ExecuteInEditMode]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Colliderless Impact")]
	public class D2dColliderlessImpact : MonoBehaviour
	{
		[Tooltip("The prefab that gets spawned once this GameObject hits a destructible")]
		public GameObject ImpactPrefab;

		private Vector3 oldPosition;

		protected virtual void OnEnable()
		{
			oldPosition = transform.position;
		}

		protected virtual void Start()
		{
			oldPosition = transform.position;
		}

		protected virtual void FixedUpdate()
		{
			var newPosition = transform.position;
			var hit         = D2dDestructible.RaycastAlphaFirst(oldPosition, newPosition);

			// Hit something?
			if (hit != null)
			{
				if (ImpactPrefab != null)
				{
					Instantiate(ImpactPrefab, hit.Position, transform.rotation);
				}

				Destroy(gameObject);
			}

			oldPosition = newPosition;
		}
	}
}

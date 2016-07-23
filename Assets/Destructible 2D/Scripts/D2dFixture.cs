using UnityEngine;
using System.Collections.Generic;

namespace Destructible2D
{
	[DisallowMultipleComponent]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Fixture")]
	public class D2dFixture : MonoBehaviour
	{
		[Tooltip("The fixture offset position")]
		public Vector3 Offset;

		[System.NonSerialized]
		private D2dDestructible destructible;

		protected virtual void OnEnable()
		{
			if (destructible              == null) destructible              = GetComponentInParent<D2dDestructible>();
			if (destructible.OnStartSplit == null) destructible.OnStartSplit = new D2dEvent();
			if (destructible.OnEndSplit   == null) destructible.OnEndSplit   = new D2dDestructibleListEvent();

			Hook();
		}

		protected virtual void OnDisable()
		{
			Unhook();
		}

		protected virtual void Update()
		{
			UpdateFixture();
		}

#if UNITY_EDITOR
		protected virtual void OnDrawGizmosSelected()
		{
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.color  = Color.red;

			Gizmos.DrawLine(Offset - Vector3.left, Offset + Vector3.left);
			Gizmos.DrawLine(Offset - Vector3.up  , Offset + Vector3.up  );
		}
#endif

		private void UpdateFixture()
		{
			if (destructible == null) destructible = GetComponentInParent<D2dDestructible>();

			if (destructible == null)
			{
				DestroyFixture();
			}
			else
			{
				var worldPosition = transform.TransformPoint(Offset);

				if (destructible.SampleAlpha(worldPosition) < 0.5f)
				{
					DestroyFixture();
				}
			}
		}

		private void DestroyFixture()
		{
			D2dHelper.Destroy(gameObject);
		}

		private void OnStartSplit()
		{
			transform.SetParent(null, false);
		}

		private void OnEndSplit(List<D2dDestructible> clones)
		{
			for (var i = clones.Count - 1; i >= 0; i--)
			{
				var clone = clones[i];

				if (TryFixTo(clone) == true)
				{
					return;
				}
			}

			DestroyFixture();
		}

		private bool TryFixTo(D2dDestructible newDestructible)
		{
			var isDifferent = destructible != newDestructible;

			// Temporarily change parent
			transform.SetParent(newDestructible.transform, false);

			// Find world position of fixture if it were attached to tempDestructible
			var worldPosition = transform.TransformPoint(Offset);

			// Can fix to new point?
			if (newDestructible.SampleAlpha(worldPosition) > 0.5f)
			{
				if (isDifferent == true)
				{
					Unhook();

					destructible = newDestructible;

					Hook();
				}

				return true;
			}

			// Change back to old parent
			transform.SetParent(destructible.transform, false);

			return false;
		}

		private void Hook()
		{

			destructible.OnStartSplit.AddListener(OnStartSplit);
			destructible.OnEndSplit  .AddListener(OnEndSplit);
		}

		private void Unhook()
		{
			destructible.OnStartSplit.RemoveListener(OnStartSplit);
			destructible.OnEndSplit  .RemoveListener(OnEndSplit);
		}
	}
}

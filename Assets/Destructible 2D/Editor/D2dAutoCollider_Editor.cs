using UnityEngine;
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dAutoCollider))]
	public class D2dAutoCollider_Editor : D2dCollider_Editor<D2dAutoCollider>
	{
		protected override void OnInspector()
		{
			base.OnInspector();
		}
	}
}
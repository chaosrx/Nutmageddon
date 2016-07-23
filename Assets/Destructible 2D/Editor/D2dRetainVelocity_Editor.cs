using UnityEngine;
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dRetainVelocity))]
	public class D2dRetainVelocity_Editor : D2dEditor<D2dRetainVelocity>
	{
		protected override void OnInspector()
		{
		}
	}
}
using UnityEngine;
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dCalculateMass))]
	public class D2dCalculateMass_Editor : D2dEditor<D2dCalculateMass>
	{
		protected override void OnInspector()
		{
			DrawDefault("MassPerSolidPixel");
		}
	}
}
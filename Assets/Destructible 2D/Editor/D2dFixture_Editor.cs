using UnityEngine;
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dFixture))]
	public class D2dFixture_Editor : D2dEditor<D2dFixture>
	{
		protected override void OnInspector()
		{
			DrawDefault("Offset");
			
			if (Any(t => t.GetComponentInParent<D2dDestructible>()) == false)
			{
				EditorGUILayout.HelpBox("Fixtures must be children of a Destructible", MessageType.Error);
			}
			
			if (Any(t => {var d = t.GetComponentInParent<D2dDestructible>(); return d == null || d.gameObject != t.gameObject; }) == false)
			{
				EditorGUILayout.HelpBox("Fixtures shouldn't be attached to the same GameObject as Destructibles", MessageType.Error);
			}
		}
	}
}
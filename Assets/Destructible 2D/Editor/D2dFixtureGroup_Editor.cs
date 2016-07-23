using UnityEngine;
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dFixtureGroup))]
	public class D2dFixtureGroup_Editor : D2dEditor<D2dFixtureGroup>
	{
		protected override void OnInspector()
		{
			DrawDefault("Fixtures");
			
			DrawDefault("AutoDestroy");
			
			EditorGUILayout.Separator();
			
			DrawDefault("OnAllFixturesRemoved");
		}
	}
}
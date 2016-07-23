using UnityEngine;
using UnityEditor;

namespace Destructible2D
{
	public class D2dCollider_Editor<T> : D2dEditor<T>
		where T : D2dCollider
	{
		private bool showEvents;

		protected override void OnInspector()
		{
			EditorGUI.BeginChangeCheck();
			{
				DrawDefault("IsTrigger");

				DrawDefault("Material");
			}
			if (EditorGUI.EndChangeCheck() == true)
			{
				serializedObject.ApplyModifiedProperties();

				Each(t => t.UpdateColliderSettings());
			}
		}
	}
}

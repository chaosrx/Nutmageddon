using UnityEngine;
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dPolygonCollider))]
	public class D2dPolygonCollider_Editor : D2dCollider_Editor<D2dPolygonCollider>
	{
		protected override void OnInspector()
		{
			EditorGUI.BeginChangeCheck();
			{
				DrawDefault("CellSize");
				
				DrawDefault("Detail");
			}
			if (EditorGUI.EndChangeCheck() == true)
			{
				Each(t => t.DestroyChild());
			}
			
			base.OnInspector();
		}
	}
}
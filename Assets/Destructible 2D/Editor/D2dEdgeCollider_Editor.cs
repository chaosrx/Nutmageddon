using UnityEngine;
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dEdgeCollider))]
	public class D2dEdgeCollider_Editor : D2dCollider_Editor<D2dEdgeCollider>
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
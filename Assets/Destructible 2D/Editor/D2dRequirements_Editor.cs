using UnityEngine;
using UnityEditor;

namespace Destructible2D
{
	[CustomEditor(typeof(D2dRequirements))]
	public class D2dRequirements_Editor : D2dEditor<D2dRequirements>
	{
		private bool showUnused;
		
		private bool showEvents;
		
		protected override void OnInspector()
		{
			showUnused = EditorGUI.Foldout(D2dHelper.Reserve(), showUnused, "Show Unused");
			
			DrawFloat("Damage Min", ref Target.UseDamageMin, ref Target.DamageMin);
			
			DrawFloat("Damage Max", ref Target.UseDamageMax, ref Target.DamageMax);
			
			DrawFloat("Alpha Count Min", ref Target.UseAlphaCountMin, ref Target.AlphaCountMin);
			
			DrawFloat("Alpha Count Max", ref Target.UseAlphaCountMax, ref Target.AlphaCountMax);
			
			DrawFloat("Remaining Alpha Min", ref Target.UseRemainingAlphaMin, ref Target.RemainingAlphaMin);
			
			DrawFloat("Remaining Alpha Max", ref Target.UseRemainingAlphaMax, ref Target.RemainingAlphaMax);
			
			EditorGUILayout.Separator();
			
			showEvents = EditorGUI.Foldout(D2dHelper.Reserve(), showEvents, "Show Events");
			
			if (showEvents == true)
			{
				DrawDefault("OnRequirementMet");
			}
		}
		
		private void DrawFloat(string title, ref bool use, ref float value)
		{
			if (use == true || showUnused == true)
			{
				var rect  = D2dHelper.Reserve();
				var right = rect; right.xMin += EditorGUIUtility.labelWidth;
				var rect1 = right; rect1.xMax = rect1.xMin + 16.0f;
				var rect2 = right; rect2.xMin += 16.0f;
				
				EditorGUI.LabelField(rect, title);
				
				EditorGUI.BeginChangeCheck();
				{
					use = EditorGUI.Toggle(rect1, "", use);
					
					value = EditorGUI.FloatField(rect2, "", value);
				}
				if (EditorGUI.EndChangeCheck() == true)
				{
					D2dHelper.SetDirty(Target);
				}
			}
		}
		
		private void DrawFloat(string title, ref bool use, ref int value)
		{
			if (use == true || showUnused == true)
			{
				var rect  = D2dHelper.Reserve();
				var right = rect; right.xMin += EditorGUIUtility.labelWidth;
				var rect1 = right; rect1.xMax = rect1.xMin + 16.0f;
				var rect2 = right; rect2.xMin += 16.0f;
				
				EditorGUI.LabelField(rect, title);
				
				EditorGUI.BeginChangeCheck();
				{
					use = EditorGUI.Toggle(rect1, "", use);
					
					value = EditorGUI.IntField(rect2, "", value);
				}
				if (EditorGUI.EndChangeCheck() == true)
				{
					D2dHelper.SetDirty(Target);
				}
			}
		}
	}
}
using UnityEngine;
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dQuadFracturer))]
	public class D2dQuadFracturer_Editor : D2dEditor<D2dQuadFracturer>
	{
		protected override void OnInspector()
		{
			DrawDefault("RequiredDamage");
			
			BeginError(Any(t => t.RequiredDamageMultiplier <= 1.0f));
			{
				DrawDefault("RequiredDamageMultiplier");
			}
			EndError();
			
			DrawDefault("FractureCount");
			
			BeginError(Any(t => t.FractureCountMultiplier <= 0.0f));
			{
				DrawDefault("FractureCountMultiplier");
			}
			EndError();
			
			EditorGUILayout.Separator();
			
			DrawDefault("RemainingFractures");
			
			DrawDefault("BlurSteps");
			
			DrawDefault("Irregularity");
		}
	}
}
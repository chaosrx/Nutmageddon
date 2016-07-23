using UnityEngine;
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dCollisionHandler))]
	public class D2dCollisionHandler_Editor : D2dEditor<D2dCollisionHandler>
	{
		private bool showEvents;

		protected override void OnInspector()
		{
			DrawDefault("DebugCollisions");

			Separator();

			DrawDefault("ImpactMask");

			DrawDefault("ImpactThreshold");

			BeginError(Any(t => t.ImpactDelay < 0.0f));
			{
				DrawDefault("ImpactDelay");
			}
			EndError();

			Separator();

			DrawDefault("DamageOnImpact");

			if (Any(t => t.DamageOnImpact == true))
			{
				BeginIndent();
				{
					DrawDefault("DamageDestructible");

					DrawDefault("DamageScale");
				}
				EndIndent();
			}
			
			Separator();

			showEvents = EditorGUI.Foldout(D2dHelper.Reserve(), showEvents, "Show Events");

			if (showEvents == true)
			{
				DrawDefault("OnImpact");
			}
		}
	}
}
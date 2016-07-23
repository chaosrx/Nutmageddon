using UnityEngine;
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dDestructible))]
	public class D2dDestructible_Editor : D2dEditor<D2dDestructible>
	{
		private bool showEvents;
		
		protected override void OnInspector()
		{
			DrawResetWith();
			DrawResetAlphaWith();
			
			Separator();
			
			DrawDefault("Indestructible");
			
			DrawDefault("RecordAlphaCount");

			DrawDefault("AutoSharpen");

			EditorGUILayout.Separator();
			
			DrawDefault("Splittable");
			
			if (Any(t => t.Splittable == true))
			{
				BeginIndent();
				{
					DrawDefault("MinSplitPixels");
					
					DrawDefault("FeatherSplit");
				}
				EndIndent();
			}
			
			EditorGUILayout.Separator();
			
			DrawDefault("MainTex");
			
			DrawDefault("DensityTex");
			
			DrawDefault("Sharpness");
			
			DrawDefault("color");
			
			DrawDamage();
			
			EditorGUILayout.Separator();
			
			BeginDisabled(true);
			{
				BeginMixed(Any(t => t.AlphaTex != Target.AlphaTex));
				{
					EditorGUI.ObjectField(D2dHelper.Reserve(), "Alpha Tex", Target.AlphaTex, typeof(Texture2D), false);
				}
				EndMixed();
				
				BeginMixed(Any(t => t.AlphaCount != Target.AlphaCount));
				{
					EditorGUI.IntField(D2dHelper.Reserve(), "Alpha Count", Target.AlphaCount);
				}
				EndMixed();
				
				if (Targets.Length == 1 && Target.RecordAlphaCount == true)
				{
					EditorGUI.IntField(D2dHelper.Reserve(), "Original Alpha Count", Target.OriginalAlphaCount);
					
					EditorGUI.ProgressBar(D2dHelper.Reserve(), Target.RemainingAlpha, "Remaining Alpha");
				}
			}
			EndDisabled();
			
			EditorGUILayout.Separator();
			
			showEvents = EditorGUI.Foldout(D2dHelper.Reserve(), showEvents, "Show Events");
			
			if (showEvents == true)
			{
				DrawDefault("OnStartSplit");
				
				DrawDefault("OnEndSplit");
				
				DrawDefault("OnDamageChanged");
				
				DrawDefault("OnAlphaDataReplaced");
				
				DrawDefault("OnAlphaDataModified");
				
				DrawDefault("OnAlphaDataSubset");
			}
		}
		
		private void DrawDamage()
		{
			BeginMixed(Any(t => t.Damage != Target.Damage));
			{
				var newDamage = EditorGUI.FloatField(D2dHelper.Reserve(), "Damage", Target.Damage);
				
				if (newDamage != Target.Damage)
				{
					Each(t => { t.Damage = newDamage; D2dHelper.SetDirty(t); });
				}
			}
			EndMixed();
		}
		
		private void DrawResetWith()
		{
			var rect  = D2dHelper.Reserve();
			var right = rect; right.xMin += EditorGUIUtility.labelWidth;
			var rect1 = right; rect1.xMax -= rect1.width / 2;
			var rect2 = right; rect2.xMin += rect2.width / 2;
			
			EditorGUI.LabelField(rect, "Replace With");
			
			var replaceSprite = (Sprite)EditorGUI.ObjectField(rect1, "", default(Object), typeof(Sprite), true);
			
			if (replaceSprite != null)
			{
				Each(t => { t.ReplaceWith(replaceSprite); D2dHelper.SetDirty(t); });
			}
			
			var replaceTexture2D = (Texture2D)EditorGUI.ObjectField(rect2, "", default(Object), typeof(Texture2D), true);
			
			if (replaceTexture2D != null)
			{
				Each(t => { t.ReplaceWith(replaceTexture2D); D2dHelper.SetDirty(t); });
			}
		}
		
		private void DrawResetAlphaWith()
		{
			var rect  = D2dHelper.Reserve();
			var right = rect; right.xMin += EditorGUIUtility.labelWidth;
			var rect1 = right; rect1.xMax -= rect1.width / 2;
			var rect2 = right; rect2.xMin += rect2.width / 2;
			
			EditorGUI.LabelField(rect, "Replace Alpha With");
			
			var replaceSprite = (Sprite)EditorGUI.ObjectField(rect1, "", default(Object), typeof(Sprite), true);
			
			if (replaceSprite != null)
			{
				Each(t => { t.ReplaceAlphaWith(replaceSprite); D2dHelper.SetDirty(t); });
			}
			
			var replaceTexture2D = (Texture2D)EditorGUI.ObjectField(rect2, "", default(Object), typeof(Texture2D), true);
			
			if (replaceTexture2D != null)
			{
				Each(t => { t.ReplaceAlphaWith(replaceTexture2D); D2dHelper.SetDirty(t); });
			}
		}
	}
}
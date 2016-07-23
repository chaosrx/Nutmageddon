using UnityEngine;
using UnityEditor;

namespace Destructible2D
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(D2dDestroyer))]
	public class D2dDestroyer_Editor : D2dEditor<D2dDestroyer>
	{
		protected override void OnInspector()
		{
			DrawDefault("Life");

			DrawDefault("FadeDuration");
		}
	}
}

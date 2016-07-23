using UnityEngine;

namespace Destructible2D
{
	// This component causes the current GameObject to follow the target Transform
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Follow")]
	public class D2dFollow : MonoBehaviour
	{
		public Transform Target;
		
		public void UpdatePosition()
		{
			if (Target != null)
			{
				var position = transform.position;
				
				position.x = Target.position.x;
				position.y = Target.position.y;
				
				transform.position = position;
			}
		}
		
		protected virtual void Update()
		{
			UpdatePosition();
		}
	}
}
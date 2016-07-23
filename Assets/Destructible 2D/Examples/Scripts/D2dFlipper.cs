using UnityEngine;

namespace Destructible2D
{
	// This component flips between two states and fires events based on it
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Flipper")]
	public class D2dFlipper : MonoBehaviour
	{
		public bool Flipped;
		
		public float FlipDelay = 1.0f;

		public D2dEvent OnFlip;

		public D2dEvent OnUnflip;

		private float cooldown;

		protected virtual void Update()
		{
			cooldown -= Time.deltaTime;

			if (cooldown <= 0.0f)
			{
				cooldown = FlipDelay;

				if (Flipped == true)
				{
					Flipped = false;

					if (OnUnflip != null) OnUnflip.Invoke();
                }
				else
				{
					Flipped = true;

					if (OnFlip != null) OnFlip.Invoke();
				}
            }
        }
	}
}
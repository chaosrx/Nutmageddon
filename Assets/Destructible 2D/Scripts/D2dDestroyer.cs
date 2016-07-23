using UnityEngine;

namespace Destructible2D
{
	// This component will automatically destroy the current GameObject if it becomes too small
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Destroyer")]
	public class D2dDestroyer : MonoBehaviour
	{
		[Tooltip("The amount of seconds it takes for this GameObject to get destroyed if it falls below the MinAlphaCount")]
		public float Life = 3.0f;

		[Tooltip("The amount of seconds it takes for the fade out animation to complete (Set to 0 for no fade)")]
		public float FadeDuration;

		//[SerializeField]
		//private Vector3 startLocalScale;

		[SerializeField]
		private Color startColor;

		[System.NonSerialized]
		private D2dDestructible destructible;

		protected virtual void Update()
		{
			Life -= Time.deltaTime;

			if (Life <= 0.0f)
			{
				Life = 0.0f;

				D2dHelper.Destroy(gameObject);
			}
			else
			{
				UpdateFade();
			}
		}

		private void UpdateFade()
		{
			if (FadeDuration > 0.0f)
			{
				if (destructible == null) destructible = GetComponent<D2dDestructible>();

				if (destructible != null)
				{
					if (FadeDuration > 0.0f && Life < FadeDuration)
					{
						var fade = Life / FadeDuration;

						if (startColor == default(Color))
						{
							startColor = destructible.Color;
						}

						var finalColor = startColor;

						finalColor.a *= fade;

						destructible.Color = finalColor;
					}
				}
			}
		}
		/*
		private void UpdateDestroy()
		{
			// Check for destroying
			if (destroying == false)
			{
				if (destructible == null) destructible = GetComponent<D2dDestructible>();

				if (destructible.AlphaCount < MinAlphaCount)
				{
					destroying = true;
				}
			}

			// Destroying?
			if (destroying == true)
			{
				if (FadeDuration > 0.0f && Life < FadeDuration)
				{
					var fade = Life / FadeDuration;

					switch (FadeStyle)
					{
						case DestroyFadeStyle.Shrink:
						{
							if (startLocalScale == default(Vector3))
							{
								startLocalScale = transform.localScale;
							}

							// Setting a zero scale might cause issues, so don't
							if (startLocalScale != Vector3.zero)
							{
								transform.localScale = startLocalScale * fade;
							}
						}
						break;

						case DestroyFadeStyle.Alpha:
						{
							if (destructible == null) destructible = GetComponent<D2dDestructible>();

							if (startColor == default(Color))
							{
								startColor = destructible.Color;
							}

							var finalColor = startColor;

							finalColor.a *= fade;

							destructible.Color = finalColor;
						}
						break;
					}
				}
			}
		}
		*/
	}
}

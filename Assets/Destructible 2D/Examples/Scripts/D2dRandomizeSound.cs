using UnityEngine;

namespace Destructible2D
{
	// This component randomly changes the sound's pitch and clip
	[DisallowMultipleComponent]
	[RequireComponent(typeof(AudioSource))]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Randomize Sound")]
	public class D2dRandomizeSound : MonoBehaviour
	{
		public float PitchMin = 0.9f;
		
		public float PitchMax = 1.1f;
		
		public AudioClip[] Clips;
		
		protected virtual void Awake()
		{
			var audioSource = GetComponent<AudioSource>();
			
			audioSource.pitch = Random.Range(PitchMin, PitchMax);
			
			if (Clips != null && Clips.Length > 0)
			{
				audioSource.clip = Clips[Random.Range(0, Clips.Length)];
			}
			
			audioSource.Play();
		}
		
#if UNITY_EDITOR
		protected virtual void Reset()
		{
			var audioSource = GetComponent<AudioSource>();
			
			audioSource.playOnAwake = false;
			
			Clips = new AudioClip[] { audioSource.clip };
		}
#endif
	}
}
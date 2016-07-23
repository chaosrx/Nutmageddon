using UnityEngine;

namespace Destructible2D
{
	// This component allows you to make the objects shake
	[ExecuteInEditMode]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Camera Shake")]
	public class D2dCameraShake : MonoBehaviour
	{
		public static float Shake;
		
		public float ShakeScale = 1.0f;
		
		public float ShakeDampening = 10.0f;
		
		public float ShakeSpeed = 10.0f;
		
		private float offsetX;
		
		private float offsetY;
		
		protected virtual void Awake()
		{
			offsetX = Random.Range(-1000.0f, 1000.0f);
			offsetY = Random.Range(-1000.0f, 1000.0f);
		}
		
		protected virtual void LateUpdate()
		{
			Shake = D2dHelper.Dampen(Shake, 0.0f, ShakeDampening, Time.deltaTime);
			
			var shakeStrength = Shake * ShakeScale;
			var shakeTime     = Time.time * ShakeSpeed;
			var offset        = Vector2.zero;
			
			offset.x = Mathf.PerlinNoise(offsetX, shakeTime) * shakeStrength;
			offset.y = Mathf.PerlinNoise(offsetY, shakeTime) * shakeStrength;
			
			transform.localPosition = offset;
		}
	}
}
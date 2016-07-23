using UnityEngine;
using System.Collections;

namespace Destructible2D
{
	// This component allows you to create moving targets that randomly flip around to become indestructible
	[ExecuteInEditMode]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Moving Target")]
	public class D2dMovingTarget : MonoBehaviour
	{
		public bool FrontShowing;
		
		public float FlipSpeed = 10.0f;
		
		public float FrontTimeMin = 1.0f;
		
		public float FrontTimeMax = 2.0f;
		
		public float BackTimeMin = 1.0f;
		
		public float BackTimeMax = 10.0f;
		
		public Vector3 StartPosition;
		
		public Vector3 EndPosition;
		
		public float MoveProgress;
		
		public float MoveSpeed;
		
		public D2dDestructible Destructible;
		
		private float cooldown;
		
		private float angle;
		
		protected virtual void Awake()
		{
			ResetCooldown();
		}
		
		protected virtual void Update()
		{
			// Update flipping if the game is playing
			if (Application.isPlaying == true)
			{
				cooldown -= Time.deltaTime;
				
				// Flip?
				if (cooldown <= 0.0f)
				{
					FrontShowing = !FrontShowing;
					
					ResetCooldown();
				}
			}
			
			// Get target angle based on flip state
			var targetAngle = FrontShowing == true ? 0.0f : 180.0f;
			
			// Slowly rotate to the target angle if the game is playing
			if (Application.isPlaying == true)
			{
				angle = D2dHelper.Dampen(angle, targetAngle, FlipSpeed, Time.deltaTime);
			}
			// Instantly rotate if it's not
			else
			{
				angle = targetAngle;
			}
			
			transform.localRotation = Quaternion.Euler(0.0f, angle, 0.0f);
			
			// Make the destructible indestructible if it's past 90 degrees
			if (Destructible != null)
			{
				Destructible.Indestructible = targetAngle >= 90.0f;
			}
			
			// Update movement
			MoveProgress += MoveSpeed * Time.deltaTime;
			
			var moveDistance = (EndPosition - StartPosition).magnitude;
			
			if (moveDistance > 0.0f)
			{
				var progress01 = Mathf.PingPong(MoveProgress / moveDistance, 1.0f);
				
				transform.localPosition = Vector3.Lerp(StartPosition, EndPosition, Mathf.SmoothStep(0.0f, 1.0f, progress01));
			}
		}
		
		protected virtual void OnDrawGizmosSelected()
		{
			if (transform.parent != null)
			{
				Gizmos.matrix = transform.parent.localToWorldMatrix;
			}
			
			Gizmos.DrawLine(StartPosition, EndPosition);
		}
		
		private void ResetCooldown()
		{
			if (FrontShowing == true)
			{
				cooldown = Random.Range(FrontTimeMin, FrontTimeMax);
			}
			else
			{
				cooldown = Random.Range(BackTimeMin, BackTimeMax);
			}
		}
	}
}
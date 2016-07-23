using UnityEngine;

namespace Destructible2D
{
	// This component allows you to control a car
	[RequireComponent(typeof(Rigidbody2D))]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Player Car")]
	public class D2dPlayerCar : MonoBehaviour
	{
		public D2dWheel[] SteerWheels;

		public float SteerAngleMax = 20.0f;

		public float SteerAngleDampening = 5.0f;
		
		public D2dWheel[] DriveWheels;

		public float DriveTorque = 1.0f;
		
		private float currentAngle;
		
		protected virtual void Update()
		{
			var targetAngle = Input.GetAxisRaw("Horizontal") * SteerAngleMax;

			currentAngle = D2dHelper.Dampen(currentAngle, targetAngle, SteerAngleDampening, Time.deltaTime);
			
			for (var i = 0; i < SteerWheels.Length; i++)
			{
				SteerWheels[i].transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -currentAngle);
			}
		}
		
		protected virtual void FixedUpdate()
		{
			for (var i = 0; i < DriveWheels.Length; i++)
			{
				DriveWheels[i].AddTorque(Input.GetAxisRaw("Vertical") * DriveTorque * Time.fixedDeltaTime);
			}
		}
	}
}
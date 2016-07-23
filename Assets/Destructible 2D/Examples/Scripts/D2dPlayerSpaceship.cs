using UnityEngine;

namespace Destructible2D
{
	// This component allows you to control a spaceship
	[RequireComponent(typeof(Rigidbody2D))]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Player Spaceship")]
	public class D2dPlayerSpaceship : MonoBehaviour
	{
		public GameObject BulletPrefab;

		public GameObject MuzzleFlashPrefab;

		public float ReloadTime;
		
		public float TurnSpeed;
		
		public float MoveSpeed;
		
		public float MaxThrusterScale;
		
		public float MaxThrusterFlicker;

		public Transform LeftGun;

		public Transform RightGun;

		public Transform LeftThruster;
		
		public Transform RightThruster;
		
		private float thrusterScale;
		
		private float cooldown;

		private int shotsFired;
		
		private Rigidbody2D mainRigidbody;
		
		protected virtual void Update()
		{
			// Cool down the gun
			cooldown -= Time.deltaTime;
			
			// Does the player want to shoot?
			if (Input.GetButton("Jump") == true)
			{
				// Is the gun ready to shoot?
				if (cooldown <= 0.0f)
				{
					cooldown    = ReloadTime;
					shotsFired += 1;

					// Find the muzzle position
					var muzzlePosition = transform.position;

					if (shotsFired % 2 == 0)
					{
						if (LeftGun != null) muzzlePosition = LeftGun.position;
					}
					else
					{
						if (RightGun != null) muzzlePosition = RightGun.position;
					}

					// Spawn bullet?
					if (BulletPrefab != null)
					{
						Instantiate(BulletPrefab, muzzlePosition, transform.rotation);
					}

					// Spawn muzzle flash?
					if (MuzzleFlashPrefab != null)
					{
						Instantiate(MuzzleFlashPrefab, muzzlePosition, transform.rotation);
					}
				}
			}
			
			UpdateThruster( LeftThruster, Input.GetAxisRaw("Horizontal") < 0.0f || Input.GetAxisRaw("Vertical") > 0.0f);
			UpdateThruster(RightThruster, Input.GetAxisRaw("Horizontal") > 0.0f || Input.GetAxisRaw("Vertical") > 0.0f);
		}
		
		protected virtual void FixedUpdate()
		{
			if (mainRigidbody == null) mainRigidbody = GetComponent<Rigidbody2D>();
			
			mainRigidbody.AddTorque(-Input.GetAxis("Horizontal") * TurnSpeed);
			
			mainRigidbody.AddRelativeForce(Vector2.up * Input.GetAxis("Vertical") * MoveSpeed);
		}
		
		private void UpdateThruster(Transform transform, bool on)
		{
			if (transform != null)
			{
				var target  = on == true ? MaxThrusterScale : 0.0f;
				var flicker = Random.Range(1.0f, 1.0f + MaxThrusterFlicker) - MaxThrusterFlicker * 0.5f;
				
				thrusterScale = Mathf.MoveTowards(thrusterScale, target, Time.deltaTime * 5.0f);
				
				transform.localScale = new Vector3(thrusterScale * flicker, thrusterScale, thrusterScale);
			}
		}
	}
}
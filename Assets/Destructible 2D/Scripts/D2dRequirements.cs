using UnityEngine;
using System.Collections.Generic;

namespace Destructible2D
{
	[RequireComponent(typeof(D2dDestructible))]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Requirements")]
	public class D2dRequirements : MonoBehaviour
	{
		public bool UseDamageMin;
		
		public float DamageMin;
		
		public bool UseDamageMax;
		
		public float DamageMax;
		
		public bool UseAlphaCountMin;
		
		public int AlphaCountMin;
		
		public bool UseAlphaCountMax;
		
		public int AlphaCountMax;
		
		public bool UseRemainingAlphaMin;
		
		public float RemainingAlphaMin = 0.0f;
		
		public bool UseRemainingAlphaMax;
		
		public float RemainingAlphaMax = 1.0f;
		
		public D2dEvent OnRequirementMet;
		
		[SerializeField]
		private bool met;
		
		[System.NonSerialized]
		private D2dDestructible destructible;
		
		public void UpdateMet()
		{
			if (met != CheckMet())
			{
				if (met == true)
				{
					met = false;
				}
				else
				{
					met = true;
					
					if (OnRequirementMet != null) OnRequirementMet.Invoke();
				}
			}
		}
		
		protected virtual void Update()
		{
			UpdateMet();
		}
		
		private bool CheckMet()
		{
			if (destructible == null) destructible = GetComponent<D2dDestructible>();
			
			if (UseDamageMin == true && destructible.Damage < DamageMin)
			{
				return false;
			}
			
			if (UseDamageMax == true && destructible.Damage > DamageMax)
			{
				return false;
			}
			
			if (UseAlphaCountMin == true && destructible.AlphaCount < AlphaCountMin)
			{
				return false;
			}
			
			if (UseAlphaCountMax == true && destructible.AlphaCount > AlphaCountMax)
			{
				return false;
			}
			
			if (UseRemainingAlphaMin == true && destructible.RemainingAlpha < RemainingAlphaMin)
			{
				return false;
			}
			
			if (UseRemainingAlphaMax == true && destructible.RemainingAlpha > RemainingAlphaMax)
			{
				return false;
			}
			
			return true;
		}
	}
}
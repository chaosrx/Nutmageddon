using UnityEngine;
using System.Collections.Generic;

namespace Destructible2D
{
	[System.Serializable]
	public class D2dPolygonColliderCell
	{
		public PolygonCollider2D Collider;
		
		[System.NonSerialized]
		private int polygonCount;
		
		public void AddPolygon(List<PolygonCollider2D> unusedColliders, GameObject child, Vector2[] points)
		{
			if (Collider == null)
			{
				if (unusedColliders != null && unusedColliders.Count > 0)
				{
					var index = unusedColliders.Count - 1;
					
					Collider = unusedColliders[index];
					
					unusedColliders.RemoveAt(index);
				}
				else
				{
					Collider = child.AddComponent<PolygonCollider2D>();
				}
			}
			
			polygonCount += 1;
			
			if (Collider.pathCount < polygonCount)
			{
				Collider.pathCount = polygonCount;
			}
			
			Collider.SetPath(polygonCount - 1, points);
		}
		
		public void Trim()
		{
			if (Collider != null && Collider.pathCount > polygonCount)
			{
				Collider.pathCount = polygonCount;
			}
		}
		
		public void Clear(List<PolygonCollider2D> unusedColliders)
		{
			if (Collider != null)
			{
				unusedColliders.Add(Collider);
				
				Collider = null;
			}
			
			polygonCount = 0;
		}
		
		public void UpdateColliderSettings(bool isTrigger, PhysicsMaterial2D material)
		{
			if (Collider != null)
			{
				Collider.isTrigger      = isTrigger;
				Collider.sharedMaterial = material;
			}
		}
	}
}
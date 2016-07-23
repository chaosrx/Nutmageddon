using UnityEngine;
using System.Collections.Generic;

namespace Destructible2D
{
	[System.Serializable]
	public class D2dEdgeColliderCell
	{
		public List<EdgeCollider2D> Colliders = new List<EdgeCollider2D>();
		
		public EdgeCollider2D AddPath(List<EdgeCollider2D> unusedColliders, GameObject child, Vector2[] points)
		{
			var index    = unusedColliders != null ? unusedColliders.FindIndex(c => c != null) : -1;
			var collider = default(EdgeCollider2D);
			
			if (index >= 0)
			{
				collider = unusedColliders[index];
				
				unusedColliders.RemoveAt(index);
			}
			else
			{
				collider = child.AddComponent<EdgeCollider2D>();
			}
			
			collider.points = points;
			
			Colliders.Add(collider);
			
			return collider;
		}
		
		public void Clear(List<EdgeCollider2D> unusedColliders)
		{
			for (var i = Colliders.Count - 1; i >= 0; i--)
			{
				var collider = Colliders[i];
				
				if (collider != null)
				{
					unusedColliders.Add(collider);
				}
			}
			
			Colliders.Clear();
		}
		
		public void UpdateColliderSettings(bool isTrigger, PhysicsMaterial2D material)
		{
			for (var i = Colliders.Count - 1; i >= 0; i--)
			{
				var collider = Colliders[i];
				
				if (collider != null)
				{
					collider.isTrigger      = isTrigger;
					collider.sharedMaterial = material;
				}
			}
		}
	}
}
using UnityEngine;
using System.Collections.Generic;

namespace Destructible2D
{
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Polygon Collider")]
	public class D2dPolygonCollider : D2dCollider
	{
		[Tooltip("The size of each collider cell")]
		[D2dPopup(8, 16, 32, 64, 128, 256)]
		public int CellSize = 64;
		
		[Tooltip("How many vertices should remain in the collider shapes")]
		[Range(0.5f, 1.0f)]
		public float Detail = 0.9f;
		
		[SerializeField]
		private int width;
		
		[SerializeField]
		private int height;
		
		[SerializeField]
		private List<D2dPolygonColliderCell> cells = new List<D2dPolygonColliderCell>();
		
		[System.NonSerialized]
		private List<PolygonCollider2D> unusedColliders = new List<PolygonCollider2D>();
		
		public override void UpdateColliderSettings()
		{
			for (var i = cells.Count - 1; i >= 0; i--)
			{
				cells[i].UpdateColliderSettings(IsTrigger, Material);
			}
		}
		
		protected override void OnAlphaDataReplaced()
		{
			base.OnAlphaDataReplaced();
			
			Rebuild();
		}
		
		protected override void OnAlphaDataModified(D2dRect rect)
		{
			base.OnAlphaDataModified(rect);
			
			if (CellSize > 0)
			{
				var cellXMin = rect.XMin / CellSize;
				var cellYMin = rect.YMin / CellSize;
				var cellXMax = (rect.XMax + 1) / CellSize;
				var cellYMax = (rect.YMax + 1) / CellSize;
				
				// Mark
				for (var cellY = cellYMin; cellY <= cellYMax; cellY++)
				{
					var offset = cellY * width;
					
					for (var cellX = cellXMin; cellX <= cellXMax; cellX++)
					{
						var index = cellX + offset;
						
						if (index >= 0 && index < cells.Count)
						{
							Mark(cells[index]);
						}
						else
						{
							Regenerate();
						}
					}
				}
				
				// Generate
				for (var cellY = cellYMin; cellY <= cellYMax; cellY++)
				{
					var offset = cellY * width;
					
					for (var cellX = cellXMin; cellX <= cellXMax; cellX++)
					{
						var index = cellX + offset;
						
						if (index >= 0 && index < cells.Count)
						{
							RebuildCell(cells[index], cellX, cellY);
						}
					}
				}
				
				Sweep();
			}
			else
			{
				Rebuild();
			}
		}
		
		protected override void OnAlphaDataSubset(D2dVector2 size, D2dVector2 offset)
		{
			base.OnAlphaDataSubset(size, offset);
			
			Rebuild();
		}
		
		protected override void OnStartSplit()
		{
			base.OnStartSplit();
			
			Mark();
			Sweep();
		}
		
		private void Mark()
		{
			for (var i = cells.Count - 1; i >= 0; i--)
			{
				D2dPool<D2dPolygonColliderCell>.Despawn(cells[i], c => c.Clear(unusedColliders));
			}
			
			cells.Clear();
		}
		
		private void Mark(D2dPolygonColliderCell cell)
		{
			cell.Clear(unusedColliders);
		}
		
		private void Sweep()
		{
			for (var i = unusedColliders.Count - 1; i >= 0; i--)
			{
				D2dHelper.Destroy(unusedColliders[i]);
			}
			
			unusedColliders.Clear();
		}
		
		private void Rebuild()
		{
			Mark();
			{
				if (CellSize > 0)
				{
					width  = (destructible.AlphaWidth  + CellSize - 1) / CellSize;
					height = (destructible.AlphaHeight + CellSize - 1) / CellSize;
					
					for (var y = 0; y < height; y++)
					{
						for (var x = 0; x < width; x++)
						{
							var cell = D2dPool<D2dPolygonColliderCell>.Spawn() ?? new D2dPolygonColliderCell();
							
							RebuildCell(cell, x, y);
							
							cells.Add(cell);
						}
					}
					
					UpdateColliderSettings();
				}
			}
			Sweep();
		}
		
		private void RebuildCell(D2dPolygonColliderCell cell, int x, int y)
		{
			var xMin = CellSize * x;
			var yMin = CellSize * y;
			var xMax = Mathf.Min(CellSize + xMin, destructible.AlphaWidth );
			var yMax = Mathf.Min(CellSize + yMin, destructible.AlphaHeight);
			
			D2dColliderBuilder.CalculatePoly(destructible.AlphaData, destructible.AlphaWidth, xMin, xMax, yMin, yMax);
			
			D2dColliderBuilder.BuildPoly(cell, unusedColliders, child, Detail);
			
			cell.UpdateColliderSettings(IsTrigger, Material);
		}
	}
}
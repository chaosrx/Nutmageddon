using UnityEngine;
using System.Collections.Generic;

namespace Destructible2D
{
	// This component automatically spawns tiles based on the main camera's orthographic size
	[ExecuteInEditMode]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Background")]
	public class D2dBackground : MonoBehaviour
	{
		public enum Axes
		{
			Horizontal,
			Vertical,
			HorizontalAndvertical
		}
		
		public D2dTile Prefab;
		
		public Axes TileAxis = Axes.HorizontalAndvertical;
		
		public Vector2 OffsetPerSecond;
		
		public Vector2 Offset;
		
		public bool OverrideSorting;
		
		public int SortingOrder;
		
		[HideInInspector]
		public List<D2dTile> Tiles;
		
		private Camera mainCamera;
		
		protected virtual void Update()
		{
			Offset += OffsetPerSecond * Time.deltaTime;
			
			UpdateTiles();
		}
		
		private void UpdateTiles()
		{
			var tileCount = 0;
			
			if (Prefab != null && Prefab.Size.x > 0.0f && Prefab.Size.y > 0.0f)
			{
				if (mainCamera == null) mainCamera = Camera.main;
				
				if (mainCamera != null && mainCamera.orthographic == true)
				{
					var width  = Mathf.CeilToInt(mainCamera.orthographicSize * mainCamera.aspect / Prefab.Size.x);
					var height = Mathf.CeilToInt(mainCamera.orthographicSize                     / Prefab.Size.y);
					
					if (TileAxis == Axes.Horizontal) height = 0;
					if (TileAxis == Axes.Vertical  ) width  = 0;
					
					for (var y = -height; y <= height; y++)
					{
						for (var x = -width; x <= width; x++)
						{
							// Expand tile array?
							if (tileCount == Tiles.Count)
							{
								Tiles.Add(null);
							}
							
							// Get tile at this index
							var tile = Tiles[tileCount];
							
							// Create tile?
							if (tile == null)
							{
								tile = Instantiate(Prefab);
								
								tile.enabled = false;
								
								tile.transform.SetParent(transform, false);
								
								Tiles[tileCount] = tile;
							}
							
							if (OverrideSorting == true)
							{
								tile.UpdateRenderer(SortingOrder);
							}
							
							// Update this tile
							tile.Offset.X = x;
							tile.Offset.Y = y;
							
							tile.UpdatePosition(Offset);
							
							// Increment tile count
							tileCount += 1;
						}
					}
				}
			}
			
			// Remove unused tiles
			for (var i = Tiles.Count - 1; i >= tileCount; i--)
			{
				var tile = Tiles[i];
				
				if (tile != null)
				{
					D2dHelper.Destroy(tile.gameObject);
				}
				
				Tiles.RemoveAt(i);
			}
		}
	}
}
using UnityEngine;
using System.Collections.Generic;

namespace Destructible2D
{
	public static class D2dFloodfill
	{
		private class Spread
		{
			public int i;
			public int x;
			public int y;
		}
		
		public static List<D2dFloodfillIsland> Islands = new List<D2dFloodfillIsland>();
		
		private static byte[] cells;
		
		private static List<Spread> spreads = new List<Spread>();
		
		private static D2dFloodfillIsland currentIsland;
		
		private static int spreadCount;
		
		private static int width;
		
		private static int height;
		
		private static int total;
		
		private const byte CELL_EMPTY = 0;
		
		private const byte CELL_SOLID = 1;
		
		private const byte CELL_CLAIM = 2;
		
		public static void Find(byte[] newData, int newWidth, int newHeight)
		{
			width       = newWidth;
			height      = newHeight;
			total       = newWidth * newHeight;
			spreadCount = 0;
			
			if (cells == null || total > cells.Length)
			{
				cells = new byte[total];
			}
			
			for (var i = Islands.Count - 1; i >= 0; i--)
			{
				D2dPool<D2dFloodfillIsland>.Despawn(Islands[i], j => j.Clear());
			}
			
			Islands.Clear();
			
			// Find all solid pixels
			for (var i = 0; i < total; i++)
			{
				cells[i] = newData[i] > 127 ? CELL_SOLID : CELL_EMPTY;
			}
			
			for (var i = 0; i < total; i++)
			{
				if (cells[i] == CELL_SOLID)
				{
					currentIsland = D2dPool<D2dFloodfillIsland>.Spawn() ?? new D2dFloodfillIsland();
					
					BeginFloodFill(i, i % newWidth, i / newWidth);
					
					Islands.Add(currentIsland);
				}
			}
		}
		
		public static void Feather(D2dFloodfillIsland island)
		{
			for (var i = island.Pixels.Count - 1; i >= 0; i--)
			{
				var pixel = island.Pixels[i];
				var x     = pixel.X;
				var y     = pixel.Y;
				
				TryFeather(island, x - 1, y    );
				TryFeather(island, x + 1, y    );
				TryFeather(island, x    , y - 1);
				TryFeather(island, x    , y + 1);
			}
		}
		
		private static void TryFeather(D2dFloodfillIsland island, int x, int y)
		{
			if (x >= 0 && y >= 0 && x < width && y < height)
			{
				var i = x + y * width;
				
				if (cells[i] == CELL_EMPTY)
				{
					cells[i] = CELL_CLAIM;
					
					island.AddPixel(x, y);
				}
			}
			else
			{
				island.AddPixel(x, y);
			}
		}
		
		private static void BeginFloodFill(int i, int x, int y)
		{
			var oldSpreadsCount = 0;
			
			SpreadTo(i, x, y);
			
			// Non-recursive floodfill
			while (spreadCount != oldSpreadsCount)
			{
				var start = oldSpreadsCount;
				var end   = oldSpreadsCount = spreadCount;
				
				for (var j = start; j < end; j++)
				{
					var spread = spreads[j];
					
					FloodFill(spread.i, spread.x, spread.y);
				}
			}
		}
		
		private static void SpreadTo(int i, int x, int y)
		{
			cells[i] = CELL_CLAIM;
			
			var spread = default(Spread);
			
			if (spreadCount >= spreads.Count)
			{
				spread = new Spread(); spreads.Add(spread);
			}
			else
			{
				spread = spreads[spreadCount];
			}
			
			spreadCount += 1;
			
			spread.i = i;
			spread.x = x;
			spread.y = y;
			
			currentIsland.AddPixel(x, y);
		}
		
		private static void FloodFill(int i, int x, int y)
		{
			// Left
			if (x > 0)
			{
				var n = i - 1;
				
				if (cells[n] == CELL_SOLID)
				{
					SpreadTo(n, x - 1, y);
				}
			}
			
			// Right
			if (x < width - 1)
			{
				var n = i + 1;
				
				if (cells[n] == CELL_SOLID)
				{
					SpreadTo(n, x + 1, y);
				}
			}
			
			// Bottom
			if (y > 0)
			{
				var n = i - width;
				
				if (cells[n] == CELL_SOLID)
				{
					SpreadTo(n, x, y - 1);
				}
			}
			
			// Top
			if (y < height - 1)
			{
				var n = i + width;
				
				if (cells[n] == CELL_SOLID)
				{
					SpreadTo(n, x, y + 1);
				}
			}
		}
	}
}
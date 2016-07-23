using UnityEngine;
using System.Collections.Generic;

namespace Destructible2D
{
	public class D2dFloodfillIsland
	{
		public List<D2dFloodfillPixel> Pixels = new List<D2dFloodfillPixel>();
		
		public void AddPixel(int x, int y)
		{
			var pixel = D2dPool<D2dFloodfillPixel>.Spawn() ?? new D2dFloodfillPixel();
			
			pixel.X = x;
			pixel.Y = y;
			
			Pixels.Add(pixel);
		}
		
		public void Clear()
		{
			for (var i = Pixels.Count - 1; i >= 0; i--)
			{
				D2dPool<D2dFloodfillPixel>.Despawn(Pixels[i]);
			}
			
			Pixels.Clear();
		}
	}
}
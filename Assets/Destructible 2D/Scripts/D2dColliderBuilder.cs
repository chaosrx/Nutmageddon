using UnityEngine;
using System.Collections.Generic;

namespace Destructible2D
{
	public static class D2dColliderBuilder
	{
		enum Edge
		{
			Left,
			Right,
			Bottom,
			Top
		}
		
		class Point
		{
			public bool Used;
			
			public Edge Inner;
			
			public Edge Outer;
			
			public Vector2 Position;
			
			public Cell Cell;
			
			public Point Other;
		}
		
		class Cell
		{
			public int X;
			
			public int Y;
			
			public int PointIndex;
			
			public int PointCount;
		}
		
		private static List<Cell> cells = new List<Cell>();
		
		private static int cellCount;
		
		public static int cellsX;
		
		public static int cellsY;
		
		private static List<Point> points = new List<Point>();
		
		private static int pointCount;
		
		private static D2dLinkedList<Point> lines = new D2dLinkedList<Point>();
		
		private static byte[] padData;
		
		private static int padWidth;
		
		private static int padHeight;
		
		public static void CalculatePoly(byte[] alphaData, int alphaWidth, int xMin, int xMax, int yMin, int yMax)
		{
			var padXMin = xMin - 1;
			var padXMax = xMax + 1;
			var padYMin = yMin - 1;
			var padYMax = yMax + 1;
			
			padWidth  = padXMax - padXMin;
			padHeight = padYMax - padYMin;
			
			if (padData == null || padData.Length < padWidth * padHeight)
			{
				padData = new byte[padWidth * padHeight];
			}
			
			for (var y = padYMin; y < padYMax; y++)
			{
				var padOffset = (y - padYMin) * padWidth;
				var alpOffset = y * alphaWidth;
				
				for (var x = padXMin; x < padXMax; x++)
				{
					var padIndex = (x - padXMin) + padOffset;
					var alpIndex = x + alpOffset;
					
					if (x >= xMin && y >= yMin && x < xMax && y < yMax)
					{
						padData[padIndex] = alphaData[alpIndex];
					}
					else
					{
						padData[padIndex] = 0;
					}
				}
			}
			
			CreateCells(padXMax - padXMin - 1, padYMax - padYMin - 1, xMin, yMin);
		}
		
		public static void BuildPoly(D2dPolygonColliderCell cell, List<PolygonCollider2D> unusedColliders, GameObject child, float detail)
		{
			cell.Clear(unusedColliders);
			
			// Find the start points of paths
			for (var i = 0; i < pointCount; i++)
			{
				var point = points[i];
				
				if (point.Used == false)
				{
					Trace(point);
					WeldLines();
					OptimizeEdges(detail);
					
					cell.AddPolygon(unusedColliders, child, ExtractPoints(1));
				}
			}
			
			// Remove unused polygons
			cell.Trim();
		}
		
		public static void CalculateEdge(byte[] alphaData, int alphaWidth, int alphaHeight, int xMin, int xMax, int yMin, int yMax)
		{
			var padXMin = xMin;
			var padXMax = xMax + 1;
			var padYMin = yMin;
			var padYMax = yMax + 1;
			
			if (xMin == 0) padXMin -= 1; else xMin += 1;
			if (yMin == 0) padYMin -= 1; else yMin += 1;
			
			padWidth  = padXMax - padXMin;
			padHeight = padYMax - padYMin;
			
			if (padData == null || padData.Length < padWidth * padHeight)
			{
				padData = new byte[padWidth * padHeight];
			}
			
			for (var y = padYMin; y < padYMax; y++)
			{
				var padOffset = (y - padYMin) * padWidth;
				var alpOffset = y * alphaWidth;
				
				for (var x = padXMin; x < padXMax; x++)
				{
					var padIndex = (x - padXMin) + padOffset;
					var alpIndex = x + alpOffset;
					
					if (x >= 0 && y >= 0 && x < alphaWidth && y < alphaHeight)
					{
						padData[padIndex] = alphaData[alpIndex];
					}
					else
					{
						padData[padIndex] = 0;
					}
				}
			}
			
			CreateCells(padXMax - padXMin - 1, padYMax - padYMin - 1, xMin, yMin);
		}
		
		public static void BuildEdge(D2dEdgeColliderCell cell, List<EdgeCollider2D> unusedColliders, GameObject child, float detail)
		{
			cell.Clear(unusedColliders);
			
			// Find the start points of paths
			for (var i = 0; i < pointCount; i++)
			{
				var point = points[i];
				
				if (point.Used == false)
				{
					Trace(point);
					WeldLines();
					OptimizeEdges(detail);
					
					cell.AddPath(unusedColliders, child, ExtractPoints(0));
				}
			}
		}
		
		private static void WeldLines()
		{
			if (lines.Count > 2)
			{
				var a = lines.First;
				var b = a.Next;
				var c = b.Next;
				var v = b.Value.Position - a.Value.Position;
				
				while (c != null)
				{
					var n = c.Value.Position - b.Value.Position;
					var z = n - v;
					
					if (z.sqrMagnitude < 0.01f)
					{
						lines.Remove(b);
					}
					else
					{
						v = n;
					}
					
					b = c;
					c = b.Next;
				}
			}
		}
		
		private static void OptimizeEdges(float detail)
		{
			if (detail < 1.0f && lines.Count > 2)
			{
				var a = lines.First;
				var b = a.Next;
				var c = b.Next;
				
				while (c != lines.Last)
				{
					var av  = a.Value.Position;
					var bv  = b.Value.Position;
					var cv  = c.Value.Position;
					var ab  = Vector3.Normalize(bv - av);
					var bc  = Vector3.Normalize(cv - bv);
					var abc = Vector3.Dot(ab, bc);
					
					if (abc > detail)
					{
						lines.Remove(b);
						
						b = c;
						c = c.Next;
					}
					else
					{
						a = b;
						b = c;
						c = c.Next;
					}
				}
			}
		}
		
		public static Vector2[] ExtractPoints(int skip)
		{
			var count = lines.Count - skip;
			
			if (count > 1)
			{
				var array = new Vector2[count];
				var node  = lines.First;
				var index = 0;
				
				while (index < count)
				{
					array[index] = node.Value.Position;
					
					node  = node.Next;
					index += 1;
				}
				
				return array;
			}
			
			return null;
		}
		
		private static void CreateCells(int width, int height, int cornerX, int cornerY)
		{
			cellCount  = 0;
			cellsX     = width;
			cellsY     = height;
			pointCount = 0;
			
			for (var y = 0; y < height; y++)
			{
				var offset = y * padWidth;
				
				for (var x = 0; x < width; x++)
				{
					var cell  = GetNextCell();
					var index = x + offset;
					var bl    = padData[index    ];
					var br    = padData[index + 1]; index += padWidth;
					var tl    = padData[index    ];
					var tr    = padData[index + 1];
					
					cell.X = x;
					cell.Y = y;
					
					CalculateCell(cell, cornerX + x, cornerY + y, bl, br, tl, tr);
				}
			}
		}
		
		private static void Trace(Point point)
		{
			var other = point.Other;
			
			// Clear line
			lines.Clear();
			
			// Trace one side of point
			Trace(point, false);
			
			// Trace other side of point
			if (other.Used == false)
			{
				Trace(other, true);
			}
		}
		
		private static void Trace(Point point, bool last)
		{
			point.Used = true;
			
			// Add point to linked list
			if (last == true)
			{
				lines.AddLast(point);
			}
			else
			{
				lines.AddFirst(point);
			}
			
			// Find adjacent point
			var cell = point.Cell;
			
			switch (point.Inner)
			{
				case Edge.Left:
				{
					cell = GetCell(cell.X - 1, cell.Y);
				}
				break;
				
				case Edge.Right:
				{
					cell = GetCell(cell.X + 1, cell.Y);
				}
				break;
				
				case Edge.Bottom:
				{
					cell = GetCell(cell.X, cell.Y - 1);
				}
				break;
				
				case Edge.Top:
				{
					cell = GetCell(cell.X, cell.Y + 1);
				}
				break;
			}
			
			if (cell != null)
			{
				for (var i = cell.PointCount - 1; i >= 0; i--)
				{
					var outerPoint = points[cell.PointIndex + i];
					
					if (outerPoint.Used == false && outerPoint.Inner == point.Outer)
					{
						outerPoint.Used = true;
						
						Trace(outerPoint.Other, last);
						
						break;
					}
				}
			}
		}
		
		private static void CalculateCell(Cell cell, int cornerX, int cornerY, float bl, float br, float tl, float tr)
		{
			var count   = 0;
			var index   = pointCount;
			var useBl   = bl >= 128;
			var useBr   = br >= 128;
			var useTl   = tl >= 128;
			var useTr   = tr >= 128;
			var offsetX = cornerX - 0.5f;
			var offsetY = cornerY - 0.5f;
			
			// Top
			if (useTl ^ useTr)
			{
				var point = GetNextPoint(); count += 1;
				
				point.Cell       = cell;
				point.Position.x = offsetX + (tl - 128) / (tl - tr);
				point.Position.y = offsetY + 1.0f;
				point.Inner      = Edge.Top;
				point.Outer      = Edge.Bottom;
			}
			
			// Right
			if (useTr ^ useBr)
			{
				var point = GetNextPoint(); count += 1;
				
				point.Cell       = cell;
				point.Position.x = offsetX + 1.0f;
				point.Position.y = offsetY + (br - 128) / (br - tr);
				point.Inner      = Edge.Right;
				point.Outer      = Edge.Left;
			}
			
			// Bottom
			if (useBl ^ useBr)
			{
				var point = GetNextPoint(); count += 1;
				
				point.Cell       = cell;
				point.Position.x = offsetX + (bl - 128) / (bl - br);
				point.Position.y = offsetY;
				point.Inner      = Edge.Bottom;
				point.Outer      = Edge.Top;
			}
			
			// Left
			if (useTl ^ useBl)
			{
				var point = GetNextPoint(); count += 1;
				
				point.Cell       = cell;
				point.Position.x = offsetX;
				point.Position.y = offsetY + (bl - 128) / (bl - tl);
				point.Inner      = Edge.Left;
				point.Outer      = Edge.Right;
			}
			
			// Pair up points
			switch (count)
			{
				case 2:
				{
					points[index + 0].Other = points[index + 1];
					points[index + 1].Other = points[index + 0];
				}
				break;
				
				case 4:
				{
					if (useTl == true && useBl == true)
					{
						points[index + 0].Other = points[index + 1];
						points[index + 1].Other = points[index + 0];
						points[index + 2].Other = points[index + 3];
						points[index + 3].Other = points[index + 2];
					}
					else
					{
						points[index + 0].Other = points[index + 3];
						points[index + 1].Other = points[index + 2];
						points[index + 2].Other = points[index + 1];
						points[index + 3].Other = points[index + 0];
					}
				}
				break;
			}
			
			cell.PointIndex = index;
			cell.PointCount = count;
		}
		
		private static Cell GetCell(int x, int y)
		{
			if (x >= 0 && y >= 0 && x < cellsX && y < cellsY)
			{
				return cells[x + y * cellsX];
			}
			
			return null;
		}
		
		private static Cell GetNextCell()
		{
			var cell = default(Cell);
			
			if (cellCount >= cells.Count)
			{
				cell = new Cell(); cells.Add(cell);
			}
			else
			{
				cell = cells[cellCount];
			}
			
			cellCount += 1;
			
			return cell;
		}
		
		private static Point GetNextPoint()
		{
			var point = default(Point);
			
			if (pointCount >= points.Count)
			{
				point = new Point(); points.Add(point);
			}
			else
			{
				point = points[pointCount];
				point.Used = false;
			}
			
			pointCount += 1;
			
			return point;
		}
	}
}
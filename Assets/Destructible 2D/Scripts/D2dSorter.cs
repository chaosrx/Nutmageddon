using UnityEngine;
using System.Collections;

namespace Destructible2D
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Renderer))]
	[AddComponentMenu(D2dHelper.ComponentMenuPrefix + "Sorter")]
	public class D2dSorter : MonoBehaviour
	{
		[System.NonSerialized]
		private Renderer tempRenderer;
		
		public int SortingOrder
		{
			set
			{
				if (tempRenderer == null) tempRenderer = GetComponent<Renderer>();
				
				tempRenderer.sortingOrder = value;
			}
			
			get
			{
				if (tempRenderer == null) tempRenderer = GetComponent<Renderer>();
				
				return tempRenderer.sortingOrder;
			}
		}
		
		public int SortingLayerID
		{
			set
			{
				if (tempRenderer == null) tempRenderer = GetComponent<Renderer>();
				
				tempRenderer.sortingLayerID = value;
			}
			
			get
			{
				if (tempRenderer == null) tempRenderer = GetComponent<Renderer>();
				
				return tempRenderer.sortingLayerID;
			}
		}
		
		public string SortingLayerName
		{
			set
			{
				if (tempRenderer == null) tempRenderer = GetComponent<Renderer>();
				
				tempRenderer.sortingLayerName = value;
			}
			
			get
			{
				if (tempRenderer == null) tempRenderer = GetComponent<Renderer>();
				
				return tempRenderer.sortingLayerName;
			}
		}
	}
}
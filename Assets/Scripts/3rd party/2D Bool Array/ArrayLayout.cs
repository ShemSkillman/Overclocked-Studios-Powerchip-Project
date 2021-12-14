using UnityEngine;
using System.Collections;

// Source: https://www.youtube.com/watch?v=uoHc-Lz9Lsc&ab_channel=SumeetKhobare

[System.Serializable]
public class ArrayLayout
{
	[System.Serializable]
	public struct rowData
	{
		public bool[] row;
	}

	public rowData[] rows = new rowData[7]; //Grid of 7x7
}
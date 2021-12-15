using UnityEngine;
using System.Collections;

// Source: https://www.youtube.com/watch?v=uoHc-Lz9Lsc&ab_channel=SumeetKhobare

[System.Serializable]
public class ArrayLayout
{
	public static int size = 4;

	[System.Serializable]
	public struct rowData
	{
		public bool[] row;
	}

	public rowData[] rows = new rowData[size];

	public bool[,] GetBoolean2DArray()
    {
		bool[,] ret = new bool[size,size];

		for (int y = 0; y < size; y++)
        {
			for (int x = 0; x < size; x++)
            {
				ret[x, y] = rows[y].row[x];
            }
        }

		return ret;
    }
}
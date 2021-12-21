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

		for (int y = size - 1; y >= 0; y--)
        {
			for (int x = 0; x < size; x++)
            {
				ret[x, size-y-1] = rows[y].row[x];
            }
        }

		return ret;
    }

	public Vector2 GetSize2D()
    {
		bool[,] boolMatrix2D = GetBoolean2DArray();

		int startX = -1, endX = -1;
		int startY = -1, endY = -1;

		for (int y = 0; y < size; y++)
		{
			for (int x = 0; x < size; x++)
			{
				if (boolMatrix2D[x, y])
                {
					if (startX == -1 || x < startX)
                    {
						startX = x;
                    }

					if (endX == -1 || x > endX)
                    {
						endX = x;
                    }

					if (startY == -1 || y < startY)
                    {
						startY = y;
                    }

					if (endY == -1 || y > endY)
					{
						endY = y;
					}
				}
			}
		}

		int sizeX = endX - startX + 1;
		int sizeY = endY - startY + 1;

		return new Vector2(sizeX, sizeY);
	}
}
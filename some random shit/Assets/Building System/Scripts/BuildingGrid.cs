/*
source: https://www.youtube.com/watch?v=waEsGu--9P8
Author: CodeMonkey
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class BuildingGrid
{
    private int width;
    private int height;
    private int length;
    private int[,,] gridArray;

    private float cellSize;

    private Vector3 originPosition;

    public Transform gridTransform;

    private Quaternion rotation;


    public BuildingGrid(int width, int height, int length, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.length = length;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        rotation = Quaternion.Euler(90, 0, 0);

        gridArray = new int[width, height, length];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                for (int z = 0; z < gridArray.GetLength(2); z++)
                {
                    Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y, z + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x + 1, y, z), Color.white, 100f);
                }
            }
        }
        Debug.DrawLine(GetWorldPosition(0, 0, length), GetWorldPosition(width, 0, length), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(width, 0, 0), GetWorldPosition(width, 0, length), Color.white, 100f);

    }

    private Vector3 GetWorldPosition(int x, int y, int z)
    {
        return new Vector3(x, y, z) * cellSize + originPosition;
    }

    private Vector3 GetXYZ(Vector3 worldPosition)
    {

        worldPosition.x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        worldPosition.y = Mathf.FloorToInt((worldPosition + originPosition).y / cellSize);
        worldPosition.z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);

        return worldPosition;
    }

    public void SetValue(int x, int y, int z, int value)
    {
        if (x >= 0 && y >= 0 && z >= 0 && x < width && y < height && z < length)
        {
            gridArray[x, y, z] += value;
            // Debug.Log("gridArray[" + x + "," + y + "," + z + "] = " + gridArray[x, y, z]);
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        worldPosition = GetXYZ(worldPosition);

        int x, y, z;

        x = (int)worldPosition.x;
        y = (int)worldPosition.y;
        z = (int)worldPosition.z;

        SetValue(x, y, z, value);

    }
}

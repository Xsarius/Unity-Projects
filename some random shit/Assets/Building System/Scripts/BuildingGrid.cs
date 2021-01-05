using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGrid
{
    private int width;
    private int height;
    private int length;
    public GameObject[,,] gridArray;

    private float cellSize;

    private Vector3 originPosition;

    public BuildingGrid(int width, int height, int length, float cellSize, Vector3 originPosition)
    {
        this.width = width;
        this.height = height;
        this.length = length;
        this.cellSize = cellSize;
        this.originPosition = originPosition;
        this.gridArray = new GameObject[width, height, length];

        /*
                for (int x = 0; x < gridArray.GetLength(0); x++)
                {
                    for (int y = 0; y < gridArray.GetLength(1); y++)
                    {
                        for (int z = 0; z < gridArray.GetLength(2); z++)
                        {
                            Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y, z + 1), Color.white, 1f, true);
                            Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x + 1, y, z), Color.white, 1f, true);
                        }
                    }
                }
                Debug.DrawLine(GetWorldPosition(0, 0, length), GetWorldPosition(width, 0, length), Color.white, 1f, true);
                Debug.DrawLine(GetWorldPosition(width, 0, 0), GetWorldPosition(width, 0, length), Color.white, 1f, true);
        */
    }

    public Vector3 GetWorldPosition(int x, int y, int z)
    {
        return new Vector3(x, y, z) * cellSize + originPosition;
    }

    public Vector3 GetXYZ(Vector3 worldPosition)
    {
        Vector3 gridPosition = new Vector3();

        gridPosition.x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        gridPosition.y = Mathf.FloorToInt((worldPosition + originPosition).y / cellSize);
        gridPosition.z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);

        return gridPosition;
    }

    public bool IsSlotFree(Vector3 position)
    {
        position = GetXYZ(position);

        int x = (int)position.x;
        int y = (int)position.y;
        int z = (int)position.z;

        if (x >= 0 && y >= 0 && z >= 0 && x < width && y < height && z < length)
        {
            if (gridArray[x, y, z] != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }

    }

    public void FreeSlot(int x, int y, int z)
    {
        gridArray[x, y, z] = null;
    }

    public void FreeSlot(Vector3 position)
    {
        FreeSlot((int)GetXYZ(position).x, (int)GetXYZ(position).y, (int)GetXYZ(position).z);
    }

    public void SetBuilding(int x, int y, int z, GameObject building)
    {
        gridArray[x, y, z] = building;

    }

    public void SetBuilding(Vector3 worldPosition, GameObject building)
    {
        SetBuilding((int)GetXYZ(worldPosition).x, (int)GetXYZ(worldPosition).y, (int)GetXYZ(worldPosition).z, building);
    }

    public GameObject FindInGrid(int x, int y, int z)
    {
        return gridArray[x, y, z];
    }

    public GameObject FindObjectInGrid(Vector3 worldPosition)
    {

        return FindInGrid((int)GetXYZ(worldPosition).x, (int)GetXYZ(worldPosition).y, (int)GetXYZ(worldPosition).z);
    }

}

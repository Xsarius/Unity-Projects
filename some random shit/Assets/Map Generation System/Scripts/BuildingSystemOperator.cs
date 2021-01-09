using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystemOperator : MonoBehaviour
{
    public Chunk chunk;

    private Vector3 mouseCurrentRay;

    public GameObject destroyer;

    public GameObject[] referenceBuildings;
    public GameObject[] buildingButtons;

    public List<GameObject> buildings;

    public void UpdateBuildingSystem()
    {

        HandleBuilding();

        if (destroyer.activeInHierarchy == true)
        {
            DeactivateBuildingButtons();
            Destroyer();
        }
        else
        {
            ActivateBuildingButtons();
        }

    }

    private void HandleBuilding()
    {
        GameObject notPlacedBuilding = null;

        foreach (GameObject building in buildings)
        {
            if (building.GetComponent<BuildingProperties>().isPlaced == false)
            {
                notPlacedBuilding = building;
                break;
            }
        }

        if (notPlacedBuilding == null)
        {
            ActivateBuildingButtons();
        }
        else
        {
            DeactivateBuildingButtons();

            notPlacedBuilding.transform.position = new Vector3(GetMouseClickPosition().x, GetMouseClickPosition().y + notPlacedBuilding.transform.localScale.y / 2, GetMouseClickPosition().z);

            Vector3 nearestCellCenter;

            nearestCellCenter = GetXYZ(notPlacedBuilding.transform.position);
            nearestCellCenter = GetWorldPosition((int)nearestCellCenter.x, (int)nearestCellCenter.y, (int)nearestCellCenter.z);
            nearestCellCenter += new Vector3(chunk.data.cellSize * .5f, 0, chunk.data.cellSize * .5f);

            notPlacedBuilding.transform.position = new Vector3(nearestCellCenter.x, notPlacedBuilding.transform.position.y, nearestCellCenter.z);

            if (Input.GetMouseButtonDown(0))
            {
                if (IsSlotFree(notPlacedBuilding.transform.position))
                {
                    SetBuilding(notPlacedBuilding.transform.position, notPlacedBuilding);
                    notPlacedBuilding.transform.position = new Vector3(nearestCellCenter.x, notPlacedBuilding.transform.position.y, nearestCellCenter.z);
                    notPlacedBuilding.GetComponent<BuildingProperties>().isPlaced = true;
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                buildings.Remove(notPlacedBuilding);
                Destroy(notPlacedBuilding);
            }
        }

    }

    private void Destroyer()
    {
        Vector3 nearestCellCenter = new Vector3();

        destroyer.transform.position = GetMouseClickPosition();

        nearestCellCenter = GetXYZ(destroyer.transform.position);                                                           // Get cell.
        nearestCellCenter = GetWorldPosition((int)nearestCellCenter.x, (int)nearestCellCenter.y, (int)nearestCellCenter.z); // Get origin position of the cell.
        nearestCellCenter += new Vector3(chunk.data.cellSize * .5f, 0, chunk.data.cellSize * .5f);                                    // Move to the center of the cell.

        destroyer.transform.position = new Vector3(nearestCellCenter.x, destroyer.transform.position.y, nearestCellCenter.z);

        if (Input.GetMouseButtonDown(0))
        {
            if (IsSlotFree(destroyer.transform.position))
            {
                Debug.Log("Slot is already free");
            }
            else
            {
                buildings.Remove(FindObjectInGrid(destroyer.transform.position));

                Destroy(FindObjectInGrid(destroyer.transform.position));

                FreeSlot(destroyer.transform.position);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            destroyer.SetActive(false);
        }
    }



    private Vector3 GetMouseClickPosition()

    {
        Vector3 mouse = new Vector3();

        Plane plane = new Plane(Vector3.up, Vector3.zero);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float entry;
        if (plane.Raycast(ray, out entry))
        {
            mouse = ray.GetPoint(entry);
        }

        return mouse;
    }

    // Building commands //

    private GameObject CreateBuilding()
    {
        GameObject newBuilding = new GameObject();

        newBuilding.AddComponent<MeshFilter>();
        newBuilding.AddComponent<MeshRenderer>();
        newBuilding.AddComponent<BuildingProperties>();

        return newBuilding;
    }

    private void CreateCylinderBuilding()
    {
        GameObject copy = CreateBuilding();

        copy.name = "new cylinder building";
        copy.transform.localScale = referenceBuildings[0].transform.localScale;
        copy.GetComponent<MeshFilter>().mesh = referenceBuildings[0].GetComponent<MeshFilter>().mesh;
        copy.GetComponent<MeshRenderer>().material = referenceBuildings[0].GetComponent<MeshRenderer>().material;
        copy.GetComponent<BuildingProperties>().isPlaced = false;
        copy.transform.position = GetMouseClickPosition();
        copy.transform.rotation = Quaternion.identity;

        buildings.Add(copy);
    }

    private void CreateCubeBuilding()
    {
        GameObject copy = CreateBuilding();

        copy.name = "new cube building";
        copy.transform.localScale = referenceBuildings[1].transform.localScale;
        copy.GetComponent<MeshFilter>().mesh = referenceBuildings[1].GetComponent<MeshFilter>().mesh;
        copy.GetComponent<MeshRenderer>().material = referenceBuildings[1].GetComponent<MeshRenderer>().material;
        copy.GetComponent<BuildingProperties>().isPlaced = false;
        copy.transform.position = GetMouseClickPosition();
        copy.transform.rotation = Quaternion.identity;

        buildings.Add(copy);
    }

    public Vector3 GetWorldPosition(int x, int y, int z)
    {
        return new Vector3(x, y, z) * chunk.data.cellSize + chunk.chunkOffset;
    }

    public Vector3 GetXYZ(Vector3 worldPosition)
    {
        Vector3 gridPosition = new Vector3();

        gridPosition.x = Mathf.FloorToInt((worldPosition - chunk.chunkOffset).x / chunk.data.cellSize);
        gridPosition.y = Mathf.FloorToInt((worldPosition + chunk.chunkOffset).y / chunk.data.cellSize);
        gridPosition.z = Mathf.FloorToInt((worldPosition - chunk.chunkOffset).z / chunk.data.cellSize);

        return gridPosition;
    }

    public bool IsSlotFree(int x, int y, int z)
    {
        if (x >= 0 && y >= 0 && z >= 0 && x < chunk.data.chunkSize.x && y < chunk.data.chunkSize.y && z < chunk.data.chunkSize.z)
        {
            if (chunk.buildingGrid[x, z] != null)
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

    public void FreeSlot(Vector3 position)
    {
        position = GetXYZ(position);

        int x = (int)position.x;
        int y = (int)position.y;
        int z = (int)position.z;

        FreeSlot(x, y, z);
    }

    public bool IsSlotFree(Vector3 position)
    {
        position = GetXYZ(position);

        int x = (int)position.x;
        int y = (int)position.y;
        int z = (int)position.z;

        return IsSlotFree(x, y, z);
    }

    public void FreeSlot(int x, int y, int z)
    {
        chunk.buildingGrid[x, z] = null;
    }

    public void SetBuilding(int x, int y, int z, GameObject building)
    {
        chunk.buildingGrid[x, z] = building;

    }

    public void SetBuilding(Vector3 position, GameObject building)
    {
        position = GetXYZ(position);

        int x = (int)position.x;
        int y = (int)position.y;
        int z = (int)position.z;

        SetBuilding(x, y, z, building);
    }

    public GameObject FindObjectInGrid(int x, int y, int z)
    {
        return chunk.buildingGrid[x, z];
    }

    public GameObject FindObjectInGrid(Vector3 position)
    {
        position = GetXYZ(position);

        int x = (int)position.x;
        int y = (int)position.y;
        int z = (int)position.z;

        return FindObjectInGrid(x, y, z);
    }

    // Buttons commands //

    public void DestroyButtonClick()
    {
        destroyer.SetActive(true);
        destroyer.transform.position = GetMouseClickPosition();
    }

    public void BuildingButton_Cylinder_Click()
    {
        CreateCylinderBuilding();
    }

    public void BuildingButton_Cube_Click()
    {
        CreateCubeBuilding();
    }

    private void ActivateBuildingButtons()
    {
        for (int i = 0; i < buildingButtons.GetLength(0); i++)
        {
            buildingButtons[i].SetActive(true);
        }
    }

    private void DeactivateBuildingButtons()
    {
        for (int i = 0; i < buildingButtons.GetLength(0); i++)
        {
            buildingButtons[i].SetActive(false);
        }
    }

}

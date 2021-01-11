using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystemOperator : MonoBehaviour
{
    ///////////////
    // Variables //
    ///////////////
    //
    // Summary:
    //     
    //
    //
    public Chunk chunk;
    //
    // Summary:
    //     
    //
    //
    private Vector3 mouseCurrentRay;
    //
    // Summary:
    //     
    //
    //
    public GameObject destroyer;
    //
    // Summary:
    //     
    //
    //
    public GameObject[] referenceBuildings;
    //
    // Summary:
    //     
    //
    //
    public GameObject[] buildingButtons;
    //
    // Summary:
    //     
    //
    //
    public List<GameObject> buildings;

    /////////////
    // Methods //
    /////////////
    //
    // Summary:
    //     
    //
    // Parameters:
    //
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
    //
    // Summary:
    //     
    //
    // Parameters:
    //
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

            notPlacedBuilding.transform.position = new Vector3(GetMouseClickPosition().x, GetMouseClickPosition().y + notPlacedBuilding.transform.localScale.y, GetMouseClickPosition().z);

            Vector3 nearestCellCenter = NearestCellCenter(notPlacedBuilding);

            notPlacedBuilding.transform.position = new Vector3(nearestCellCenter.x, nearestCellCenter.y + notPlacedBuilding.transform.localScale.y, nearestCellCenter.z);

            if (Input.GetMouseButtonDown(0))
            {
                if (IsSlotFree(GetXYZ(notPlacedBuilding.transform.position)))
                {
                    SetBuilding(GetXYZ(notPlacedBuilding.transform.position), notPlacedBuilding);
                    notPlacedBuilding.transform.position = new Vector3(nearestCellCenter.x, nearestCellCenter.y + notPlacedBuilding.transform.localScale.y, nearestCellCenter.z);
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
    //
    // Summary:
    //     
    //
    // Parameters:
    //
    private void Destroyer()
    {

        destroyer.transform.position = GetMouseClickPosition();

        Vector3 nearestCellCenter = NearestCellCenter(destroyer);

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
    //
    // Summary:
    //     
    //
    // Parameters:
    //   
    //     
    //
    //  
    //     
    //
    // Returns:
    //     
    //     
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
    //
    // Summary:
    //     
    //
    // Parameters:
    //   
    //     
    //
    //  
    //     
    //
    // Returns:
    //     
    //    
    private Vector3 NearestCellCenter(GameObject objectToPosition)
    {
        Vector3 nearestCellCenter;

        nearestCellCenter = GetXYZ(objectToPosition.transform.position);
        nearestCellCenter = GetWorldPosition((int)nearestCellCenter.x, (int)nearestCellCenter.y, (int)nearestCellCenter.z);
        nearestCellCenter += new Vector3(chunk.data.cellSize * .5f, 0, chunk.data.cellSize * .5f);

        return nearestCellCenter;
    }

    // Building commands 
    //
    // Summary:
    //     
    //
    // Parameters:
    //   
    //     
    //
    //  
    //     
    //
    // Returns:
    //     
    //    
    private GameObject CreateBuilding()
    {
        GameObject newBuilding = new GameObject();

        newBuilding.AddComponent<MeshFilter>();
        newBuilding.AddComponent<MeshRenderer>();
        newBuilding.AddComponent<BuildingProperties>();

        return newBuilding;
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //
    private void CreateCylinderBuilding()
    {
        GameObject copy = CreateBuilding();

        copy.name = "Cylinder Building " + 0;
        copy.transform.localScale = referenceBuildings[0].transform.localScale;
        copy.GetComponent<MeshFilter>().mesh = referenceBuildings[0].GetComponent<MeshFilter>().mesh;
        copy.GetComponent<MeshRenderer>().material = referenceBuildings[0].GetComponent<MeshRenderer>().material;
        copy.GetComponent<BuildingProperties>().isPlaced = false;
        copy.transform.position = GetMouseClickPosition();
        copy.transform.rotation = Quaternion.identity;

        buildings.Add(copy);
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //
    private void CreateCubeBuilding()
    {
        GameObject copy = CreateBuilding();

        copy.name = "Cube Building " + 0;
        copy.transform.localScale = referenceBuildings[1].transform.localScale;
        copy.GetComponent<MeshFilter>().mesh = referenceBuildings[1].GetComponent<MeshFilter>().mesh;
        copy.GetComponent<MeshRenderer>().material = referenceBuildings[1].GetComponent<MeshRenderer>().material;
        copy.GetComponent<BuildingProperties>().isPlaced = false;
        copy.transform.position = GetMouseClickPosition();
        copy.transform.rotation = Quaternion.identity;

        buildings.Add(copy);
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //   
    //     
    //
    //  
    //     
    //
    // Returns:
    //     
    //
    public Vector3 GetWorldPosition(int x, int y, int z)
    {
        return (new Vector3(x, y, z) + chunk.chunkOffset) * chunk.data.cellSize;
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //   
    //     
    //
    //  
    //     
    //
    // Returns:
    //     
    //
    public Vector3 GetXYZ(Vector3 position)
    {
        Vector3 gridPosition;

        gridPosition.x = Mathf.FloorToInt((position - chunk.chunkOffset * chunk.data.cellSize).x / chunk.data.cellSize);
        gridPosition.y = Mathf.FloorToInt((position + chunk.chunkOffset * chunk.data.cellSize).y / chunk.data.cellSize);
        gridPosition.z = Mathf.FloorToInt((position - chunk.chunkOffset * chunk.data.cellSize).z / chunk.data.cellSize);

        return gridPosition;
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //
    public bool IsSlotFree(int x, int y, int z)
    {
        if (x >= 0 && z >= 0 && x <= chunk.data.chunkSize.x - 1 && z <= chunk.data.chunkSize.z - 1)
        {
            if (chunk.buildingGrid.grid[x, y, z] != null)
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
    //
    // Summary:
    //     
    //
    // Parameters:
    //
    public bool IsSlotFree(Vector3 position)
    {
        position = GetXYZ(position);

        int x = (int)position.x;
        int y = (int)position.y;
        int z = (int)position.z;

        return IsSlotFree(x, y, z);
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //
    public void FreeSlot(Vector3 position)
    {
        position = GetXYZ(position);

        int x = (int)position.x;
        int y = (int)position.y;
        int z = (int)position.z;

        FreeSlot(x, y, z);
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //
    public void FreeSlot(int x, int y, int z)
    {
        chunk.buildingGrid.grid[x, y, z] = null;
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //
    public void SetBuilding(int x, int y, int z, GameObject building)
    {
        chunk.buildingGrid.grid[x, y, z] = building;
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //
    public void SetBuilding(Vector3 position, GameObject building)
    {
        position = GetXYZ(position);

        int x = (int)position.x;
        int y = (int)position.y;
        int z = (int)position.z;

        SetBuilding(x, y, z, building);
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //   
    //     
    //
    //  
    //     
    //
    // Returns:
    //     GameObject located in a grid at x,y,z coordinates, ex. grid[x,y,z]
    //
    public GameObject FindObjectInGrid(int x, int y, int z)
    {
        return chunk.buildingGrid.grid[x, y, z];
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //   
    //     
    //
    //  
    //     
    //
    // Returns:
    //     
    //
    public GameObject FindObjectInGrid(Vector3 position)
    {
        position = GetXYZ(position);

        int x = (int)position.x;
        int y = (int)position.y;
        int z = (int)position.z;

        return FindObjectInGrid(x, y, z);
    }

    // Buttons commands //
    //
    // Summary:
    //     
    //
    // Parameters:
    //
    public void DestroyButtonClick()
    {
        destroyer.SetActive(true);
        destroyer.transform.position = GetMouseClickPosition();
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //
    public void BuildingButton_Cylinder_Click()
    {
        CreateCylinderBuilding();
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //
    public void BuildingButton_Cube_Click()
    {
        CreateCubeBuilding();
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //
    private void ActivateBuildingButtons()
    {
        for (int i = 0; i < buildingButtons.GetLength(0); i++)
        {
            buildingButtons[i].SetActive(true);
        }
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //
    private void DeactivateBuildingButtons()
    {
        for (int i = 0; i < buildingButtons.GetLength(0); i++)
        {
            buildingButtons[i].SetActive(false);
        }
    }

}

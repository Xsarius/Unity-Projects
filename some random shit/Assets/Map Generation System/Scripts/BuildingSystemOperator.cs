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
    //     Custom chunk class element
    //      on which all of the building 
    //      related operations are performed.
    //
    public Chunk chunk;
    //
    // Summary:
    //     Position of the mouse pointer   
    //      raycasted on to terrain / map.
    //
    private Vector3 mouseCurrentRay;
    //
    // Summary:
    //     GameObject used to destroy placed buildings.     
    //
    public GameObject destroyer;
    //
    // Summary:
    //     Array containing prefabs of the buildings.     
    //
    public GameObject[] referenceBuildings;
    //
    // Summary:
    //     Array containing the UI building buttons.
    //
    public GameObject[] buildingButtons;
    //
    // Summary:
    //     List of all created buildings.    
    //      (Instaciated as a GameObject)
    //
    public List<GameObject> buildings;
    //
    // Summary:
    //     List of all created roads.    
    //      (Instaciated as a GameObject)
    //
    public List<GameObject> roads;

    /////////////
    // Methods //
    /////////////
    //
    // Summary:
    //     
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

            notPlacedBuilding.transform.position = new Vector3(GetMouseRaycastPosition().x, GetMouseRaycastPosition().y + notPlacedBuilding.transform.localScale.y, GetMouseRaycastPosition().z);

            Vector3 nearestCellCenter = NearestCellCenter(notPlacedBuilding);

            notPlacedBuilding.transform.position = new Vector3(nearestCellCenter.x, notPlacedBuilding.transform.localScale.y, nearestCellCenter.z);

            if (Input.GetMouseButtonDown(0))
            {
                if (IsSlotFree(notPlacedBuilding.transform.position))
                {
                    SetBuilding(notPlacedBuilding.transform.position, notPlacedBuilding);
                    notPlacedBuilding.transform.position = new Vector3(nearestCellCenter.x, notPlacedBuilding.transform.localScale.y, nearestCellCenter.z);
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
    private void Destroyer()
    {

        destroyer.transform.position = GetMouseRaycastPosition();

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
    //     Function uses Ray and Plane 
    //      classes to calculate the mouse 
    //      pointer position in a function
    //      cast time instance.
    //
    // Returns:
    //     Position of the mouse pointer
    //      raycasted on the terrain / map,
    //      as a Vector3 element.
    //
    private Vector3 GetMouseRaycastPosition()

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
    //     Function calculates nearest cell
    //      center to given input object in
    //      a function cast time instance. 
    //
    // Parameters:
    //     objectToPosition - input object
    //      for which cell center will be calculated.
    //
    // Returns:
    //     Position of the nearest cell center 
    //      to given <parameter> object as a 
    //      Vector3 element.
    private Vector3 NearestCellCenter(GameObject objectToPosition)
    {
        Vector3 nearestCellCenter;

        nearestCellCenter = GetXYZ(objectToPosition.transform.position);
        nearestCellCenter = GetWorldPosition((int)nearestCellCenter.x, (int)nearestCellCenter.y, (int)nearestCellCenter.z);
        nearestCellCenter += new Vector3(chunk.data.cellSize * .5f, 0, chunk.data.cellSize * .5f);

        return nearestCellCenter;
    }

    // Building commands //
    //
    // Summary:
    //     
    // Returns:
    //     GameObject element with a basic components
    //      MeshFilter,MeshRenderer and a custom class
    //      component: BuildingProperties.
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
    private void CreateCylinderBuilding()
    {
        GameObject copy = CreateBuilding();

        copy.name = "Cylinder Building " + 0;
        copy.transform.localScale = referenceBuildings[0].transform.localScale;
        copy.GetComponent<MeshFilter>().mesh = referenceBuildings[0].GetComponent<MeshFilter>().mesh;
        copy.GetComponent<MeshRenderer>().material = referenceBuildings[0].GetComponent<MeshRenderer>().material;
        copy.GetComponent<BuildingProperties>().isPlaced = false;
        copy.transform.position = GetMouseRaycastPosition();
        copy.transform.rotation = Quaternion.identity;

        buildings.Add(copy);
    }
    //
    // Summary:
    //     
    //
    private void CreateCubeBuilding()
    {
        GameObject copy = CreateBuilding();

        copy.name = "Cube Building " + 0;
        copy.transform.localScale = referenceBuildings[1].transform.localScale;
        copy.GetComponent<MeshFilter>().mesh = referenceBuildings[1].GetComponent<MeshFilter>().mesh;
        copy.GetComponent<MeshRenderer>().material = referenceBuildings[1].GetComponent<MeshRenderer>().material;
        copy.GetComponent<BuildingProperties>().isPlaced = false;
        copy.transform.position = GetMouseRaycastPosition();
        copy.transform.rotation = Quaternion.identity;

        buildings.Add(copy);
    }
    //
    // Summary:
    //
    //
    private void CreateRoad()
    {
        GameObject roadTile = CreateBuilding();

        roadTile.name = "Road" + roads.Capacity;

        roads.Add(roadTile);
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //      x -
    //     
    //      y -
    //  
    //      z -
    //
    // Returns:
    //     
    //
    public Vector3 GetWorldPosition(int x, int y, int z)
    {
        return new Vector3(x, y, z) * chunk.data.cellSize + chunk.chunkOffset;
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //      objectPosition - 
    //     
    //
    //  
    //     
    //
    // Returns:
    //     
    //
    public Vector3 GetXYZ(Vector3 objectPosition)
    {
        Vector3 gridPosition = new Vector3();

        gridPosition.x = Mathf.FloorToInt((objectPosition - chunk.chunkOffset).x / chunk.data.cellSize);
        gridPosition.y = Mathf.FloorToInt((objectPosition + chunk.chunkOffset).y / chunk.data.cellSize);
        gridPosition.z = Mathf.FloorToInt((objectPosition - chunk.chunkOffset).z / chunk.data.cellSize);

        return gridPosition;
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //      x -
    //
    //      y -
    //
    //      z -
    //
    //  Returns:
    //  
    //
    private bool IsSlotFree(int x, int y, int z)
    {
        if (x >= 0 && z >= 0 && x <= chunk.data.chunkSize.x - 1 && z <= chunk.data.chunkSize.z - 1)
        {
            if (chunk.buildingGrid.grid[x, z] != null)
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
    //      positions - 
    //
    //
    //  Returns:
    //
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
    //      position - 
    //
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
    //      x -
    //
    //      y -
    //
    //      z -
    //
    //
    private void FreeSlot(int x, int y, int z)
    {
        chunk.buildingGrid.grid[x, z] = null;
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //      x -
    //
    //      y -
    //
    //      z -
    //
    //      building -
    //
    //
    //
    private void SetBuilding(int x, int y, int z, GameObject building)
    {
        chunk.buildingGrid.grid[x, z] = building;
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //      position - 
    //
    //
    //      building -
    //
    //
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
    //      x -
    //     
    //      y - 
    //  
    //      z -
    //
    // Returns:
    //     GameObject located in a grid at x,z coordinates, ex. grid[x,z].
    //
    private GameObject FindObjectInGrid(int x, int y, int z)
    {
        return chunk.buildingGrid.grid[x, z];
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //      position -
    //     
    //
    // Returns:
    //     GameObject located in a grid at x,z coordinates, ex. grid[x,z].
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
    //     Activating all buttons stored 
    //      in the buildingButtons[] array.
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
    //     Deactivating all buttons stored 
    //      in the buildingButtons[] array.
    //
    private void DeactivateBuildingButtons()
    {
        for (int i = 0; i < buildingButtons.GetLength(0); i++)
        {
            buildingButtons[i].SetActive(false);
        }
    }
    //
    // Summary:
    //     Action to be performed on 
    //      destroy button click.
    //
    public void Button_Destroy()
    {
        destroyer.SetActive(true);
        destroyer.transform.position = GetMouseRaycastPosition();
    }
    //
    // Summary:
    //     Action to be performed on 
    //      cylinder button click.    
    //
    public void BuildingButton_Cylinder()
    {
        CreateCylinderBuilding();
    }
    //
    // Summary:
    //     Action to be performed on 
    //      cube button click.
    //
    public void BuildingButton_Cube()
    {
        CreateCubeBuilding();
    }
    //
    // Summary:
    //
    //
    public void BuildingButton_Road()
    {

    }


}

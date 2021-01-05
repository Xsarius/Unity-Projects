using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildingCommandHandler : MonoBehaviour
{

    public float cellSize;

    private Vector3 mouseCurrentRay;

    private BuildingGrid grid;

    public Transform mapObject;

    public GameObject destroyer;

    public GameObject[] referenceBuildings;
    public GameObject[] buildingButtons;

    public List<GameObject> buildings;

    public BuildingGeneralSO buildingsGeneralData;

    private void Start()
    {
        CreateGrid();

        destroyer.SetActive(false);

    }

    private void Update()
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

    private void CreateGrid()
    {
        Vector3 originPosition;

        originPosition.x = mapObject.position.x - mapObject.localScale.x * .5f;
        originPosition.y = mapObject.position.y + mapObject.localScale.y * .5f;
        originPosition.z = mapObject.position.z - mapObject.localScale.z * .5f;

        Vector3 gridSize;

        gridSize.x = mapObject.localScale.x / cellSize;
        gridSize.y = mapObject.localScale.y / cellSize;
        gridSize.z = mapObject.localScale.z / cellSize;

        if (gridSize.x < 1)
        {
            gridSize.x = 1;
        }
        if (gridSize.y < 1)
        {
            gridSize.y = 1;
        }
        if (gridSize.z < 1)
        {
            gridSize.z = 1;
        }

        grid = new BuildingGrid((int)gridSize.x, (int)gridSize.y, (int)gridSize.z, cellSize, originPosition);

    }

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

            Vector3 nearestCellCenter;

            nearestCellCenter = grid.GetXYZ(notPlacedBuilding.transform.position);
            nearestCellCenter = grid.GetWorldPosition((int)nearestCellCenter.x, (int)nearestCellCenter.y, (int)nearestCellCenter.z);
            nearestCellCenter += new Vector3(cellSize * .5f, 0, cellSize * .5f);

            notPlacedBuilding.transform.position = new Vector3(nearestCellCenter.x, notPlacedBuilding.transform.position.y, nearestCellCenter.z);

            if (Input.GetMouseButtonDown(0))
            {
                if (grid.IsSlotFree(notPlacedBuilding.transform.position))
                {
                    grid.SetBuilding(notPlacedBuilding.transform.position, notPlacedBuilding);
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

        nearestCellCenter = grid.GetXYZ(destroyer.transform.position);

        nearestCellCenter = grid.GetWorldPosition((int)nearestCellCenter.x, (int)nearestCellCenter.y, (int)nearestCellCenter.z);

        nearestCellCenter += new Vector3(cellSize * .5f, 0, cellSize * .5f);

        destroyer.transform.position = new Vector3(nearestCellCenter.x, destroyer.transform.position.y, nearestCellCenter.z);

        if (Input.GetMouseButtonDown(0))
        {
            if (grid.IsSlotFree(destroyer.transform.position))
            {
                Debug.Log("Slot is already free");
            }
            else
            {
                buildings.Remove(grid.FindObjectInGrid(destroyer.transform.position));

                Destroy(grid.FindObjectInGrid(destroyer.transform.position));

                grid.FreeSlot(destroyer.transform.position);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            destroyer.SetActive(false);
        }
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

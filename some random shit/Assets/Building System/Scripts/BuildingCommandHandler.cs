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

    public GameObject[] reference;
    public GameObject[] buildingButtons;

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

    private void HandleBuilding()
    {
        GameObject[] notPlacedBuilding = GameObject.FindGameObjectsWithTag("notPlaced");

        if (notPlacedBuilding.GetLength(0) > 0)
        {
            DeactivateBuildingButtons();

            notPlacedBuilding[0].transform.position = new Vector3(GetMouseClickPosition().x, GetMouseClickPosition().y + notPlacedBuilding[0].transform.localScale.y, GetMouseClickPosition().z);

            PlaceBuilding(notPlacedBuilding[0]);

        }
        else
        {
            ActivateBuildingButtons();
        }
    }

    private void PlaceBuilding(GameObject building)
    {

        Vector3 nearestCellCenter;

        nearestCellCenter = grid.GetXYZ(building.transform.position);
        nearestCellCenter = grid.GetWorldPosition((int)nearestCellCenter.x, (int)nearestCellCenter.y, (int)nearestCellCenter.z);
        nearestCellCenter += new Vector3(cellSize * .5f, 0, cellSize * .5f);

        building.transform.position = new Vector3(nearestCellCenter.x, building.transform.position.y, nearestCellCenter.z);

        if (Input.GetMouseButtonDown(0))
        {
            if (grid.IsSlotFree(building.transform.position))
            {
                grid.SetBuilding(building.transform.position, building);
                building.transform.position = new Vector3(nearestCellCenter.x, building.transform.position.y, nearestCellCenter.z);
                building.tag = "placed";
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Destroy(building);
        }

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

    public void DestroyButtonClick()
    {
        destroyer.SetActive(true);
        destroyer.transform.position = GetMouseClickPosition();
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
                grid.FreeSlot(destroyer.transform.position);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            destroyer.SetActive(false);
        }
    }

    public void BuildingButton_Cylinder_Click()
    {
        GameObject copy = Instantiate(reference[0], GetMouseClickPosition(), Quaternion.identity);

        copy.tag = "notPlaced";
    }

    public void BuildingButton_Cube_Click()
    {
        GameObject copy = Instantiate(reference[1], GetMouseClickPosition(), Quaternion.identity);

        copy.tag = "notPlaced";
    }

}

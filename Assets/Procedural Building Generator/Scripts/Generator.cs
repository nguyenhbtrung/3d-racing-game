using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Generator : MonoBehaviour
{
    [SerializeField] private bool showMesh = true;
    [SerializeField] private bool customSize = false;
    [SerializeField] private int dimensions;
    [SerializeField] private int nRoad;
    [SerializeField] private int scale = 20;
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject landPrefab;
    [SerializeField] private GameObject buildingPrefab;

    private List<Cell> cells;
    private int maxTurns = 4;
    private RoadGenerator roadGenerator;
    private BuildingGenerator buildingGenerator;
    private Transform area;
    private static int areaID = 0;
    private bool needMeshUpdate = false;


    private void Awake()
    {
        if (!Application.isPlaying) return;
        
    }
    private void OnValidate()
    {
        needMeshUpdate = true;
        EditorApplication.delayCall += UpdateMeshVisibility;
    }

    private void UpdateMeshVisibility()
    {
#if UNITY_EDITOR
        if (!needMeshUpdate || this == null)
        {
            return;
        }
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = showMesh;
        }
        needMeshUpdate = false;
#endif
    }

    public void Initialize()
    {
#if UNITY_EDITOR
        if (area != null)
            DestroyImmediate(area.gameObject);
        cells = new List<Cell>();
        roadGenerator = GetComponent<RoadGenerator>();
        buildingGenerator = GetComponent<BuildingGenerator>();
        showMesh = false;
        needMeshUpdate = true;
        UpdateMeshVisibility();
#endif
    }

    public void InitGrid()
    {
        Resize();
        for (int y = 0; y < dimensions; y++)
        {
            for (int x = 0; x < dimensions; x++)
            {
                float posX = transform.position.x + (x - dimensions / 2) * scale;
                float posZ = transform.position.z + (y - dimensions / 2) * scale;
                float posY = transform.position.y;
                Vector3 position = new Vector3(posX, posY, posZ);
                Cell newCell = Instantiate(cellPrefab, position, Quaternion.identity, this.transform)
                    .GetComponent<Cell>();
                newCell.X = x;
                newCell.Y = y;
                newCell.CellType = CellType.Land;
                cells.Add(newCell);
            }
        }

        //StartCoroutine(CalculateTilesData());
        CalculateTilesData();
    }

    private void Resize()
    {
        if (customSize)
        {
            return;
        }
        if (transform.localScale.x != transform.localScale.y)
        {
            Debug.LogWarning("Scale X and Y should be equal.");
        }
        float size = Mathf.Min(transform.localScale.x, transform.localScale.y);
        dimensions = (int)size / scale;
        nRoad = (dimensions / 4) + 1;
    }

    public void CalculateTilesData()
    {
        for (int i = 0; i < nRoad; i++)
        {
            ProcessRoad();
            //yield return null;
        }
        ProcessBuilding();
        GenerateTiles();
    }

    private void ProcessRoad()
    {
        int nTurn = Random.Range(0, maxTurns) + 1;
        int currentPosX = 0;
        int currentPosY = 0;
        GetRandomStartPos(ref currentPosX, ref currentPosY);
        Cell startCell = GetCellAtPosition(currentPosX, currentPosY);
        startCell.CellType = CellType.Road;

        int turnCount = 0;
        int stepCount = 0;
        int nforwardStep = GetRandomRoadSteps();
        int direction = GetNewDirection(currentPosX, currentPosY);
        while (stepCount < dimensions * dimensions)
        {
            if (stepCount == nforwardStep && turnCount < nTurn)
            {
                stepCount = 0;
                nforwardStep = GetRandomRoadSteps();
                direction = GetNewDirection(currentPosX, currentPosY);
                turnCount++;
            }
            GetNextPos(ref currentPosX, ref currentPosY, direction);
            if (currentPosX < 0 || currentPosY < 0 || currentPosX >= dimensions || currentPosY >= dimensions)
            {
                break;
            }
            Cell currentCell = GetCellAtPosition(currentPosX, currentPosY);
            currentCell.CellType = CellType.Road;
            stepCount++;
        }
    }

    public void ProcessBuilding()
    {
        foreach (Cell cell in cells)
        {
            if (cell.CellType == CellType.Road)
            {
                continue;
            }
            if (cell.X == 0 || cell.Y == 0 || cell.X == dimensions - 1 || cell.Y == dimensions - 1)
            {
                cell.CellType = CellType.Building;
            }

            Cell upCell = GetNeighbourCell(cell, NeighbourCell.Up);
            Cell downCell = GetNeighbourCell(cell, NeighbourCell.Down);
            Cell leftCell = GetNeighbourCell(cell, NeighbourCell.Left);
            Cell rightCell = GetNeighbourCell(cell, NeighbourCell.Right);

            bool hasNeighbourRoad = IsRoad(upCell) || IsRoad(downCell) || IsRoad(leftCell) || IsRoad(rightCell);

            if (hasNeighbourRoad)
            {
                cell.CellType = CellType.Building;
            }
        }
    }

    public Cell GetCellAtPosition(int x, int y)
    {
        return cells.Find(cell => cell.X == x && cell.Y == y);
    }

    public void GenerateTiles()
    {
        Transform parent = new GameObject($"Area_{areaID:00}").GetComponent<Transform>();
        area = parent;
        foreach (Cell cell in cells)
        {
            if (cell.CellType == CellType.Road)
            {
                Cell upCell = GetNeighbourCell(cell, NeighbourCell.Up);
                Cell downCell = GetNeighbourCell(cell, NeighbourCell.Down);
                Cell leftCell = GetNeighbourCell(cell, NeighbourCell.Left);
                Cell rightCell = GetNeighbourCell(cell, NeighbourCell.Right);

                GameObject roadPrefab = roadGenerator.GetRoadPrefabs(
                    IsRoad(upCell), 
                    IsRoad(downCell), 
                    IsRoad(leftCell), 
                    IsRoad(rightCell)
                    );

                Instantiate(roadPrefab, cell.transform.position, roadPrefab.transform.rotation, parent);
            }
            else if (cell.CellType == CellType.Land || cell.CellType == CellType.Building)
            {
                Instantiate(landPrefab, cell.transform.position, landPrefab.transform.rotation, parent);
            }

            //else if (cell.CellType == CellType.Building)
            //{
            //    Instantiate(buildingPrefab, cell.transform.position, buildingPrefab.transform.rotation);
            //}

        }
        //StartCoroutine(buildingGenerator.GenerateBuildings(cells));
        buildingGenerator.GenerateBuildings(cells, parent);
        foreach (Cell cell in cells)
        {
            DestroyImmediate(cell.gameObject);
        }
        //Destroy(gameObject);
    }

    public int GetRandomRoadSteps()
    {
        int minSteps = 2;
        int maxStep = dimensions / 3 * 2;
        return Random.Range(minSteps, maxStep);
    }

    public void GetNextPos(ref int x, ref int y, int direction)
    {
        switch (direction)
        {
            case 0: y += 1; break; //up
            case 1: y -= 1; break; //down
            case 2: x += 1; break; //right
            case 3: x -= 1; break; //left
            default:
                Debug.LogWarning("Invalid direction");
                break;
        }
    }

    public void GetRandomStartPos(ref int x, ref int y)
    {
        x = 0;
        y = 0;
        int selection = Random.Range(0, 2);
        if (selection == 0)
        {
            x = Random.Range(1, dimensions);
        }
        else
        {
            y = Random.Range(1, dimensions);
        }
    }

    public int GetNewDirection(int x, int y)
    {
        int direction = Random.Range(0, 4);
        if (y == 0)
        {
            direction = 0;
        }
        if (y == dimensions - 1)
        {
            direction = 1;
        }
        if (x == 0)
        {
            direction = 2;
        }
        if (x == dimensions - 1)
        {
            direction = 3;
        }
        return direction;
    }
    
    public Cell GetNeighbourCell(Cell current, NeighbourCell neighbourCell)
    {
        switch (neighbourCell)
        {
            case NeighbourCell.Up:
                return cells.Find(c => c.X == current.X && c.Y == current.Y + 1);
            case NeighbourCell.Down:
                return cells.Find(c => c.X == current.X && c.Y == current.Y - 1);
            case NeighbourCell.Left:
                return cells.Find(c => c.X == current.X - 1 && c.Y == current.Y);
            case NeighbourCell.Right:
                return cells.Find(c => c.X == current.X + 1 && c.Y == current.Y);
            default:
                return null;
        }
    }

    public bool IsRoad(Cell cell)
    {
        return cell == null || cell.CellType == CellType.Road;
    }

    public void ApplyArea()
    {
        Debug.Log($"{area.name} has been applied");
        areaID++;
        area = null;
    }
}

public enum NeighbourCell
{
    Up,
    Down,
    Left,
    Right
}

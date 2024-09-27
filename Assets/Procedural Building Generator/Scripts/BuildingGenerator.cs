using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BuildingGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> size1Buildings;
    [SerializeField] private List<GameObject> size2Buildings;
    [SerializeField] private List<GameObject> size3Buildings;
    [SerializeField] private int maxSize = 2;

    private Transform buildingsParent;

    public void GenerateBuildings(List<Cell> cellsGrid ,Transform buildingsParent)
    {
        this.buildingsParent = buildingsParent;
        List<Cell> buildingCells = cellsGrid.Where(cell => cell.CellType == CellType.Building).ToList();
        List<Cell> usedCells = cellsGrid.Where(cell => cell.CellType == CellType.Road).ToList();
        foreach (Cell cell in buildingCells)
        {
            if (usedCells.Contains(cell))
            {
                continue;
            }
            int size = Random.Range(1, maxSize + 1);
            if (size == 3)
            {
                bool success = TryGenerateSize3(cell, cellsGrid, usedCells);
                if (!success)
                {
                    size--;
                }
            }
            if (size == 2)
            {
                bool success = TryGenerateSize2(cell, cellsGrid, usedCells);
                if (!success)
                {
                    size--;
                }
            }
            if (size == 1)
            {
                GenerateSize1(cell, cellsGrid, usedCells); 
            }
            //yield return null;
        }
    }

    private void GenerateSize1(Cell current, List<Cell> cellsGrid, List<Cell> usedCells)
    {
        int buildingIndex = Random.Range(0, size1Buildings.Count);
        Quaternion buildingRot = CalculateBuildingRotation(current, cellsGrid);
        Instantiate(size1Buildings[buildingIndex], current.transform.position, buildingRot, buildingsParent);
        usedCells.Add(current);
    }

    private bool TryGenerateSize2(Cell current, List<Cell> cellsGrid, List<Cell> usedCells)
    {
        for (int i = 0; i < size2ConsiderNeighbour.GetLength(0); i++)
        {
            bool isValid = true;
            List<Cell> considerCellList = new List<Cell>();
            for (int j = 0; j < size2ConsiderNeighbour.GetLength(1); j++)
            {
                int x = current.X + size2ConsiderNeighbour[i, j, 0];
                int y = current.Y + size2ConsiderNeighbour[i, j, 1];
                Cell considerCell = cellsGrid.Find(c => c.X == x && c.Y == y);
                if (considerCell == null || usedCells.Contains(considerCell))
                {
                    isValid = false;
                    break;
                }
                considerCellList.Add(considerCell);
            }
            if (!isValid)
            {
                continue;
            }
            int buildingIndex = Random.Range(0, size2Buildings.Count);
            Vector3 buildingPos = (current.transform.position + considerCellList[1].transform.position) / 2.0f;
            Quaternion buildingRot = CalculateBuildingRotation(current, cellsGrid);
            Instantiate(size2Buildings[buildingIndex], buildingPos, buildingRot, buildingsParent);
            usedCells.Add(current);
            usedCells.AddRange(considerCellList);
            return true;
        }
        return false;
    }

    private bool TryGenerateSize3(Cell current, List<Cell> cellsGrid, List<Cell> usedCells)
    {
        for (int i = 0; i < size3ConsiderNeighbour.GetLength(0); i++)
        {
            bool isValid = true;
            List<Cell> considerCellList = new List<Cell>();
            for (int j = 0; j < size3ConsiderNeighbour.GetLength(1); j++)
            {
                int x = current.X + size3ConsiderNeighbour[i, j, 0];
                int y = current.Y + size3ConsiderNeighbour[i, j, 1];
                Cell considerCell = cellsGrid.Find(c => c.X == x && c.Y == y);
                if (usedCells.Contains(considerCell))
                {
                    isValid = false;
                    break;
                }
                considerCellList.Add(considerCell);
            }
            if (!isValid)
            {
                continue;
            }
            int buildingIndex = Random.Range(0, size3Buildings.Count);
            Vector3 buildingPos = considerCellList[3].transform.position;
            Quaternion buildingRot = CalculateBuildingRotation(current, cellsGrid);
            Instantiate(size3Buildings[buildingIndex], buildingPos, buildingRot, buildingsParent);
            usedCells.Add(current);
            usedCells.AddRange(considerCellList);
            return true;
        }
        return false;
    }

    private Quaternion CalculateBuildingRotation(Cell current, List<Cell> cellsGrid)
    {
        Cell upCell = cellsGrid.Find(c => c.X == current.X && c.Y == current.Y + 1);
        if (upCell == null || upCell.CellType == CellType.Road)
        {
            return Quaternion.Euler(0, 0, 0);
        }
        Cell downCell = cellsGrid.Find(c => c.X == current.X && c.Y == current.Y - 1);
        if (downCell == null || downCell.CellType == CellType.Road)
        {
            return Quaternion.Euler(0, 180, 0);
        }
        Cell leftCell = cellsGrid.Find(c => c.X == current.X - 1 && c.Y == current.Y);
        if (leftCell == null || leftCell.CellType == CellType.Road)
        {
            return Quaternion.Euler(0, 270, 0);
        }
        Cell rightCell = cellsGrid.Find(c => c.X == current.X + 1 && c.Y == current.Y);
        if (rightCell == null || rightCell.CellType == CellType.Road)
        {
            return Quaternion.Euler(0, 90, 0);
        }
        return Quaternion.identity;
    }

    private int[,,] size3ConsiderNeighbour =
    {
        { { -1, 0 }, { 1, 0 }, { -1, 1 }, { 0, 1 }, { 1, 1 }, { -1, 2 }, { 0, 2 }, { 1, 2 } },
        { { -1, 0 }, { 1, 0 }, { -1, -1 }, { 0, -1}, { 1, -1}, { -1, -2 }, { 0, -2 }, { 1, -2 } },
        { { 0, 1 }, { 0, -1 }, { -1, 1 }, { -1 , 0 }, { -1, -1 }, { -2, 1}, { -2, 0 }, { -2, -1 } },
        { { 0, 1 }, { 0, -1 }, { 1, 1 }, { 1, 0 }, { 1, -1 }, { 2, 1 }, { 2, 0 }, { 2, -1 } }
    };

    private int[,,] size2ConsiderNeighbour =
    {
        { { -1, 0 }, { -1, 1 }, { 0, 1 } },
        { { 1, 0 }, { 1, 1 }, { 0, 1 } },
        { { -1, 0 }, { -1, -1 }, { 0, -1 } },
        { { 1, 0 }, { 1, -1 }, { 0, -1 } },
    };

}

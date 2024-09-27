using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private CellType cellType;
    private int x;
    private int y;

    public CellType CellType { get => cellType; set => cellType = value; }
    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }

    //private void Awake()
    //{
    //    CellType = CellType.Land;
    //}
}

public enum CellType
{
    Road,
    Land,
    Building
}

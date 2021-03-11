using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MineSweeperBoard : BoardController
{
  public Color cellColor;
  public bool[,] bombs;

  private void OnValidate()
  {
    bombs = GenerateBombs();
    PopulateBoard((cellInstance, x, y) =>
    {
      cellInstance.GetComponent<CellController>().UpdateCellColor(cellColor);
      cellInstance.GetComponent<CellController>().hasBomb = bombs[x, y];
    });

  }

  private bool[,] GenerateBombs()
  {
    var amountOfBombs = 10;
    var bombs = new bool[columns, rows];

    var coordinates = GetPossibleCoordinates().ToList();
    var rand = new System.Random();
    while (amountOfBombs-- > 0)
    {
      var coordinate = coordinates.ElementAt(rand.Next(coordinates.Count()));
      bombs[coordinate.x, coordinate.y] = true;
    }

    return bombs;
  }

  private IEnumerable<Vector2Int> GetPossibleCoordinates()
  {
    return Enumerable.Range(0, columns)
    .Select(columnIndex => Enumerable.Range(0, rows).Select(rowIndex => new Vector2Int(columnIndex, rowIndex)))
    .SelectMany(x => x);
  }
}
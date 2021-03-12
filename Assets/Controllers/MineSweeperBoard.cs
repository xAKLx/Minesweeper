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
    PopulateBoard((cellInstance, position) =>
    {
      cellInstance.GetComponent<CellController>().UpdateCellColor(cellColor);
      cellInstance.GetComponent<CellController>().hasBomb = bombs[position.x, position.y];
      cellInstance.GetComponent<CellController>().UpdateAdjacentBombAmount(CalculateAdjacentBombs(position));

    });

  }

  private int CalculateAdjacentBombs(Vector2Int position)
  {
    var bombs = new bool[] {
      getBombOrFalse(position + Vector2Int.up),
      getBombOrFalse(position + Vector2Int.up + Vector2Int.right),
      getBombOrFalse(position + Vector2Int.right),
      getBombOrFalse(position + Vector2Int.right + Vector2Int.down),
      getBombOrFalse(position + Vector2Int.down),
      getBombOrFalse(position + Vector2Int.down + Vector2Int.left),
      getBombOrFalse(position + Vector2Int.left),
      getBombOrFalse(position + Vector2Int.left + Vector2Int.up),
    };

    return bombs.Where(x => x).Count();
  }

  private bool getBombOrFalse(Vector2Int position)
  {
    return !isOutOfBounds(position) && bombs[position.x, position.y];
  }

  private bool isOutOfBounds(Vector2Int position)
  {
    return position.x < 0 || position.x >= columns || position.y < 0 || position.y >= rows;
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
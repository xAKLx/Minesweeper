using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class MineSweeperBoard : BoardController
{
  public Color cellColor;
  public bool[,] bombs;
  public GameObject[,] cells;
  public GameObject remaningBombsDisplay;
  public int amountOfBombs = 10;

  private void Start()
  {
    bombs = GenerateBombs();
    Enumerable.Range(0, columns)
    .ToList().ForEach(columnIndex => Enumerable.Range(0, rows).ToList().ForEach(rowIndex =>
    {
      cells[columnIndex, rowIndex] = transform.GetChild((rowIndex * columns) + columnIndex).gameObject;
      var cellInstance = cells[columnIndex, rowIndex];
      var cellController = cellInstance.GetComponent<CellController>();
      var position = new Vector2Int(columnIndex, rowIndex);
      cellController.openAdjacent = OpenAdjacentFactory(position);
      cellController.board = this;
      cellController.hasBomb = bombs[columnIndex, rowIndex];
      cellController.UpdateAdjacentBombAmount(CalculateAdjacentBombs(position));
      cellController.areAllAdjacentBombsFlagged = AreAllAdjacentBombsFlaggedCheckerFactory(cellController, position);

    }));
  }

  private Func<bool> AreAllAdjacentBombsFlaggedCheckerFactory(CellController cellController, Vector2Int position)
  {
    return () =>
    {
      var adjacentCells = GetAdjacentCoordinates(position)
        .Where(coordinate => !isOutOfBounds(coordinate))
        .Select(coordinate => cells[coordinate.x, coordinate.y]);
      return cellController.amountOfAdjacentBombs == adjacentCells.Where(x => x.GetComponent<CellController>().state == CellState.Flagged).Count();
    };
  }

  public void UpdateRemainingBombAmount()
  {
    var remainingBombAmount = CalculateRemainingBombAmount();
    remaningBombsDisplay.GetComponentInChildren<TextMeshPro>().text = remainingBombAmount.ToString();
  }

  public int CalculateRemainingBombAmount()
  {
    return amountOfBombs - GetAmountOfFlaggedCells();
  }

  public int GetAmountOfFlaggedCells()
  {
    return Enumerable.Range(0, columns)
      .ToList().Select(columnIndex => Enumerable.Range(0, rows).ToList().Select(rowIndex => cells[columnIndex, rowIndex]))
      .SelectMany(x => x)
      .Select(x => x.GetComponent<CellController>())
      .Where(x => x.state == CellState.Flagged)
      .Count();
  }

  private void OnValidate()
  {
    bombs = GenerateBombs();
    cells = new GameObject[columns, rows];
    PopulateBoard((cellInstance, position) =>
    {
      var cellController = cellInstance.GetComponent<CellController>();
      cellController.UpdateCellColor(cellColor);
      cellController.hasBomb = bombs[position.x, position.y];
      cellController.UpdateAdjacentBombAmount(CalculateAdjacentBombs(position));
      cells[position.x, position.y] = cellInstance;
    });

  }

  private Action OpenAdjacentFactory(Vector2Int position)
  {
    return () =>
    {
      var coordinates = GetAdjacentCoordinates(position);
      coordinates.Where(x => !isOutOfBounds(x)).ToList().ForEach(coordinate =>
      {
        cells[coordinate.x, coordinate.y].GetComponent<CellController>().Open();
      });
    };
  }

  private Vector2Int[] GetAdjacentCoordinates(Vector2Int position)
  {
    return new Vector2Int[] {
        position + Vector2Int.up,
        position + Vector2Int.up + Vector2Int.right,
        position + Vector2Int.right,
        position + Vector2Int.right + Vector2Int.down,
        position + Vector2Int.down,
        position + Vector2Int.down + Vector2Int.left,
        position + Vector2Int.left,
        position + Vector2Int.left + Vector2Int.up,
      };
  }

  private int CalculateAdjacentBombs(Vector2Int position)
  {
    var bombs = GetAdjacentCoordinates(position).Select(getBombOrFalse);
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
    var amountOfBombs = this.amountOfBombs;
    var bombs = new bool[columns, rows];

    var coordinates = GetPossibleCoordinates().ToList();
    var rand = new System.Random();
    while (amountOfBombs > 0)
    {
      var coordinate = coordinates.ElementAt(rand.Next(coordinates.Count()));
      amountOfBombs -= bombs[coordinate.x, coordinate.y] ? 0 : 1;
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
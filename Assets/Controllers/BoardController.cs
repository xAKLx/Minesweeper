using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BoardController : MonoBehaviour
{
  [Min(1)]
  public int columns = 4;
  [Min(1)]
  public int rows = 4;
  public GameObject cell;

  protected void PopulateBoard(Action<GameObject, int, int> onCellCreation)
  {
    if (EditorApplication.isPlayingOrWillChangePlaymode) return;
    AdjustCellQuantity();
    PopulateCellData(onCellCreation);
  }

  private void PopulateCellData(Action<GameObject, int, int> onCellCreation)
  {
    foreach (var rowIndex in Enumerable.Range(0, rows))
    {
      foreach (var columnIndex in Enumerable.Range(0, columns))
      {
        UnityEditor.EditorApplication.delayCall += () =>
        {
          var cellInstance = transform.GetChild((rowIndex * columns) + columnIndex).gameObject;
          cellInstance.transform.position = new Vector3(columnIndex - (columns / 2.0f) + 0.5f, rowIndex - (rows / 2.0f) + 0.5f, 0.0f);
          onCellCreation(cellInstance, columnIndex, rowIndex);
        };
      }
    }
  }

  private void AdjustCellQuantity()
  {
    var difference = (columns * rows) - transform.childCount;

    if (difference > 0)
    {
      AddNewCells(difference);
    }
    else if (difference < 0)
    {
      RemoveCells(-difference);
    }
  }

  private void AddNewCells(int amount)
  {
    Enumerable.Range(0, amount).ToList().ForEach((index) =>
    {
      var cellInstance = Instantiate(cell, new Vector3(), Quaternion.identity);
      cellInstance.transform.parent = this.transform;
    });
  }

  private void RemoveCells(int amount)
  {
    Enumerable.Range(0, amount).ToList().ForEach((index) =>
    {
      var child = transform.GetChild(transform.childCount - 1 - index);
      UnityEditor.EditorApplication.delayCall += () =>
      {
        if (child != null)
          DestroyImmediate(child.gameObject);
      };
    });
  }
}

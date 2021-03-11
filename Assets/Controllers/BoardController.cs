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
    foreach (Transform child in this.transform)
    {
      UnityEditor.EditorApplication.delayCall += () =>
      {
        if (child != null)
          DestroyImmediate(child.gameObject);
      };
    }

    foreach (var rowIndex in Enumerable.Range(0, rows))
    {
      foreach (var columnIndex in Enumerable.Range(0, columns))
      {
        UnityEditor.EditorApplication.delayCall += () =>
        {
          var cellInstance = Instantiate(cell, new Vector3(columnIndex - (columns / 2.0f) + 0.5f, rowIndex - (rows / 2.0f) + 0.5f, 0.0f), Quaternion.identity);
          cellInstance.transform.parent = this.transform;
          onCellCreation(cellInstance, columnIndex, rowIndex);
        };
      }
    }
  }
}

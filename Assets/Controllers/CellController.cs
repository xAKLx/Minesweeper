using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{
  public Color cellColor;
  public GameObject gfx;

  private void OnValidate()
  {
    gfx.GetComponent<SpriteRenderer>().color = cellColor;
  }

  public void UpdateCellColor(Color newColor)
  {
    cellColor = newColor;
    gfx.GetComponent<SpriteRenderer>().color = cellColor;
  }
}

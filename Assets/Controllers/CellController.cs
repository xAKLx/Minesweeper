using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellController : MonoBehaviour
{
  public Color cellColor;
  public GameObject gfx;
  public bool hasBomb;

  private void OnValidate()
  {
    gfx.GetComponent<SpriteRenderer>().color = cellColor;
  }

  public void UpdateCellColor(Color newColor)
  {
    cellColor = newColor;
    gfx.GetComponent<SpriteRenderer>().color = cellColor;
  }

  private void OnDrawGizmos()
  {
    if (!hasBomb) return;
    Gizmos.color = Color.black;
    Gizmos.DrawSphere(transform.position, 0.3f);
  }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum CellState
{
  Closed = 0,
  Opened = 1
}

public class CellController : MonoBehaviour
{
  public Color cellColor;
  public GameObject gfx;
  public GameObject gfx_opened;
  public bool hasBomb;
  public int amountOfAdjacentBombs = 5;
  private CellState _state = CellState.Closed;

  public CellState state
  {
    get => _state;
    set
    {
      _state = value;
      GetComponent<Animator>().SetInteger("state", (int)_state);
    }
  }

  private void OnMouseUpAsButton()
  {
    state = CellState.Opened;
  }

  private void OnValidate()
  {
    UpdateCellColor(cellColor);
    UpdateAdjacentBombAmount(amountOfAdjacentBombs);
  }

  public void UpdateAdjacentBombAmount(int amount)
  {
    amountOfAdjacentBombs = amount;
    GetComponentInChildren<TextMeshPro>(true).text = amount == 0 ? "" : amount.ToString();
  }

  public void UpdateCellColor(Color newColor)
  {
    cellColor = newColor;
    gfx.GetComponent<SpriteRenderer>().color = cellColor;
    gfx_opened.GetComponent<SpriteRenderer>().color = cellColor;
  }

  private void OnDrawGizmos()
  {
    if (!hasBomb) return;
    Gizmos.color = Color.black;
    Gizmos.DrawSphere(transform.position, 0.3f);
  }
}

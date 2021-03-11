using UnityEngine;

public class MineSweeperBoard : BoardController
{
  public Color cellColor;

  private void OnValidate()
  {
    PopulateBoard(cellInstance => cellInstance.GetComponent<CellController>().UpdateCellColor(cellColor));

  }
}
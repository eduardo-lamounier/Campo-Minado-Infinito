namespace CampoMinado.Core;

using System.Diagnostics;

public record class SafeCell : Cell
{
  public int NearBombs
  {
    get;
    set
    {
      Debug.Assert(value >= 0 && value < 9);
      field = value;
    }
  }

  public SafeCell(int nearBombs = 0)
  {
    NearBombs = nearBombs;
  }
}

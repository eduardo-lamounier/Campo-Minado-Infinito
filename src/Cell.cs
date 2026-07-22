using System.Diagnostics;

namespace CampoMinado.Grid;

public record class Cell
{
  private bool _initialized = false;

  public bool HaveFlag { get; set; } = false;
  public bool HaveBomb => NearBombs == -1;
  public bool GotInitialized => _initialized;
  public int NearBombs
  {
    get;
    set
    {
      Debug.Assert(value >= 0 && value <= 8);
      _initialized = true;
      field = value;
    }
  }

  public void PlaceBomb()
  {
    _initialized = true;
    NearBombs = -1;
  }

  public Cell() { }
}

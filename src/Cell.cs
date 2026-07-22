using System.Diagnostics;

namespace CampoMinado.Grid;

public record class Cell
{
  private bool _initialized = false;
  private bool _revealed = false;

  public bool HaveFlag { get; set; } = false;
  public bool HaveBomb => NearBombs == -1;
  public bool GotInitialized => _initialized;
  public bool IsRevealed => _revealed;

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

  // Marca a célula como revelada para o jogador
  //
  // Uma célula que não foi inicializada não deve
  // ser revelada
  public void Reveal()
  {
    Debug.Assert(_initialized);
    _revealed = true;
  }

  public Cell() { }
}

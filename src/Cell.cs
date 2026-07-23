using System.Diagnostics;

namespace CampoMinado.Core;

public record class Cell
{
  private bool _initialized = false;
  private bool _revealed = false;

  public bool HaveFlag { get; set; } = false;
  public bool HaveBomb => NearBombs == -1;
  public bool GotInitialized => _initialized;
  public bool IsRevealed => _revealed;

  // Armazena a quantidade de bombas ao redor dessa célula
  //
  // Settar essa propriedade inicializa a célula
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

  // Marca a célula como uma célula com bomba
  //
  // Inicializa a célula
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

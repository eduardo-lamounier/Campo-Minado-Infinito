using System.Diagnostics;

namespace CampoMinado.Core;

public abstract record class Cell
{
  private bool _revealed = false;
  private bool _hasFlag = false;

  public bool HasFlag => _hasFlag;
  public bool IsRevealed => _revealed;

  public void EnsureFlag()
  {
    Debug.Assert(!_revealed);
    _hasFlag = true;
  }

  public void EnsureNoFlag()
  {
    _hasFlag = false;
  }

  // Marca a célula como revelada para o jogador
  //
  // Uma célula que não foi inicializada não deve
  // ser revelada
  public void Reveal()
  {
    Debug.Assert(!_hasFlag);

    _revealed = true;
  }

  public Cell() { }
}

using System.Diagnostics;

namespace CampoMinado.Core;

public abstract record class Cell
{
  public bool HasFlag { get; private set; }
  public bool IsRevealed { get; private set; }

  public void EnsureFlag()
  {
    Debug.Assert(!IsRevealed);
    HasFlag = true;
  }

  public void EnsureNoFlag()
  {
    HasFlag = false;
  }

  // Marca a célula como revelada para o jogador
  //
  // Uma célula que não foi inicializada não deve
  // ser revelada
  public void Reveal()
  {
    Debug.Assert(!HasFlag);

    IsRevealed = true;
  }

  public Cell() { }
}

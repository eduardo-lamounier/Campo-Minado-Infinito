using System.Diagnostics;

namespace CampoMinado.Core;

/// <summary>
/// Representa uma célula genérica ou não inicializada do campo minado.
/// </summary>
public abstract record class Cell
{
  /// <summary>
  /// Retorna verdadeiro caso a célula tenha uma bandeira tenha sido colocada pelo jogador
  /// falso caso contrário.
  /// </summary>
  public bool HasFlag { get; private set; }

  /// <summary>
  /// Retorna verdadeiro caso a célula tenha sido revelada pelo jogador, ou falso caso
  /// contrário.
  /// </summary>
  public bool IsRevealed { get; private set; }

  /// <summary>
  /// Garante que nenhuma bandeira estará nessa célula
  /// </summary>
  /// <remarks>
  /// NÃO deve ser usada se a célula tiver sido revelada.
  /// </remarks>
  public void EnsureFlag()
  {
    Debug.Assert(!IsRevealed);
    HasFlag = true;
  }

  /// <summary>
  /// Garante que nenhuma bandeira estará nessa célula.
  /// </summary>
  public void EnsureNoFlag()
  {
    HasFlag = false;
  }

  /// <summary>
  /// Marca a célula como revelada para o jogador
  ///
  /// Uma célula que não foi inicializada não deve
  /// ser revelada
  /// </summary>
  public void Reveal()
  {
    Debug.Assert(!HasFlag);

    IsRevealed = true;
  }

  public Cell() { }
}

using CampoMinado.Core.Exceptions;
using System.Text.Json.Serialization;

namespace CampoMinado.Core;

/// <summary>
/// Representa uma célula genérica ou não inicializada do campo minado.
/// </summary>
[JsonDerivedType(typeof(SafeCell), "safe")]
[JsonDerivedType(typeof(DangerousCell), "dangerous")]
public abstract record class Cell
{
  /// <summary>
  /// Retorna verdadeiro caso a célula tenha uma bandeira tenha sido colocada pelo jogador
  /// falso caso contrário.
  /// </summary>
  [JsonInclude]
  public bool HasFlag { get; private set; }

  /// <summary>
  /// Retorna verdadeiro caso a célula tenha sido revelada pelo jogador, ou falso caso
  /// contrário.
  /// </summary>
  [JsonInclude]
  public bool IsRevealed { get; private set; }

  /// <summary>
  /// Garante que uma bandeira estará nessa célula.
  /// </summary>
  /// <exception cref="InvalidCellOperationException">
  /// Lançada se a célula já tiver sido revelada.
  /// </exception>
  public void EnsureFlag()
  {
    if (IsRevealed)
      throw new InvalidCellOperationException(
        "Não é possível colocar bandeira em uma célula já revelada."
      );

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
  /// Marca a célula como revelada para o jogador.
  /// </summary>
  /// <exception cref="InvalidCellOperationException">
  /// Lançada se a célula ainda tiver bandeira.
  /// </exception>
  public void Reveal()
  {
    if (HasFlag)
      throw new InvalidCellOperationException(
        "Não é possível revelar uma célula que ainda possui bandeira."
      );

    IsRevealed = true;
  }

  public Cell() { }
}

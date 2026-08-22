using CampoMinado.Core.Exceptions;

namespace CampoMinado.Core;

/// <summary>
/// Representa uma célula sem bomba do campo minado. Armazena a quantidade células com
/// bomba adjacentes.
/// </summary>
public record class SafeCell : Cell
{
  /// <summary>
  /// Representa a quantidade de células com bomba perto dessa célula.
  /// </summary>
  /// <remarks>
  /// DEVE ser um número entre 0 e 8, afinal não tem como haver uma quantidade negativa de
  /// células adjacentes ou uma quantidade maior que 8.
  /// </remarks>
  /// <exception cref="InvalidNearBombsException">
  /// Lançada ao atribuir um valor fora do intervalo [0, 8].
  /// </exception>
  public int NearBombs
  {
    get;
    set
    {
      if (value < 0 || value > 8)
        throw new InvalidNearBombsException(value);

      field = value;
    }
  }

  /// <param name="nearBombs">
  ///   Representa a quantidade de bombas adjacentes. É zero por padrão.
  /// </param>
  public SafeCell(int nearBombs = 0)
  {
    NearBombs = nearBombs;
  }
}

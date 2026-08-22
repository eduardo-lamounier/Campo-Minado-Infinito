namespace CampoMinado.Core.Exceptions;

/// <summary>
/// Lançada quando a densidade de bombas especificada para o <see cref="MineField"/>
/// está fora do intervalo [0, 1].
/// </summary>
public class InvalidBombDensityException(double attemptedValue)
  : Exception(
    $"Densidade de bombas inválida: {attemptedValue}. Esperado valor entre 0 e 1."
  )
{ }

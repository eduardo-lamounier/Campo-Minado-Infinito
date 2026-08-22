namespace CampoMinado.Core.Exceptions;

/// <summary>
/// Lançada quando a quantidade de bombas adjacentes de uma <see cref="SafeCell"/>
/// está fora do intervalo válido [0, 8].
/// </summary>
public class InvalidNearBombsException(int attemptedValue)
  : Exception(
    $"Quantidade de bombas adjacentes inválida: {attemptedValue}. Esperado valor entre 0 e 8."
  )
{ }

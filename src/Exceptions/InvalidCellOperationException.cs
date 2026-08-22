namespace CampoMinado.Core.Exceptions;

/// <summary>
/// Lançada quando uma operação sobre uma célula viola as regras do jogo
/// (ex.: revelar célula com bandeira, ou colocar bandeira em célula já revelada).
/// </summary>
/// <param name="message">
///   Mensagem que explica qual foi a operação inválida.
/// </param>
public class InvalidCellOperationException(string message) : Exception(message) { }

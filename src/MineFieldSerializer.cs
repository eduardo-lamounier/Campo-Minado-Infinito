using System.Text.Json;
using CampoMinado.Core;

namespace CampoMinado.src;

/// <summary>
/// Classe responsável pela serialização do mapa.
/// </summary>
public sealed class MineFieldSerializer
{
  /// <summary>
  /// Privado para não haver perigo de haver duas instâncias
  /// </summary>
  private MineFieldSerializer(MineField field)
  {
    Field = field;
  }

  /// <summary>
  /// Única instância da classe
  /// </summary>
  private static MineFieldSerializer? s_instance;

  /// <summary>
  /// O campo que será serializado e deserializado
  /// </summary>
  public MineField Field { get; private set; }

  private readonly JsonSerializerOptions Options = new() { WriteIndented = true };

  private readonly string Filename = "MineField.json";

  /// <summary>
  /// Retorna a instância da classe.
  /// Caso não houver uma instância, cria-a, se tiver retorna a instância criada na primeira vez
  /// </summary>
  public static MineFieldSerializer GetInstance(MineField field)
  {
    if (s_instance == null)
    {
      s_instance = new MineFieldSerializer(field);

      _ = File.Create(s_instance.Filename);
    }

    return s_instance;
  }

  /// <summary>
  /// Serializa o mapa, adicionando ele no arquivo JSON MineField.json
  /// </summary>
  /// <exception cref="InvalidOperationException">
  /// Lançada caso o serializador não tenha sido inicializado.
  /// </exception>
  /// <exception cref="FileNotFoundException">
  /// Lançada caso o arquivo JSON não possa ser encontrado.
  /// </exception>
  public static void Serialize()
  {
    if (s_instance is null)
    {
      throw new InvalidOperationException("O serializador não foi inicializado.");
    }

    if (File.Exists(s_instance.Filename))
    {
      throw new FileNotFoundException("O arquivo JSON não foi encontrado.");
    }

    var option = s_instance.Options;
    string serialization = JsonSerializer.Serialize(s_instance.Field, option);

    File.WriteAllText(s_instance.Filename, serialization);
  }

  /// <summary>
  /// Deserializa o campo do mapa
  /// </summary>
  /// <returns> Retorna o Campo do mapa que estava serializado </returns>
  /// <exception cref="InvalidOperationException">
  /// Lançada caso o serializador não tenha sido inicializado.
  /// </exception>
  /// <exception cref="FileNotFoundException">
  /// Lançada caso o arquivo JSON não possa ser encontrado.
  /// </exception>
  public static MineField? Deserialize()
  {
    if (s_instance is null)
    {
      throw new InvalidOperationException("O serializador não foi inicializado.");
    }

    if (File.Exists(s_instance.Filename))
    {
      throw new FileNotFoundException("O arquivo JSON não foi encontrado.");
    }

    string deseralization = File.ReadAllText(s_instance.Filename);

    return JsonSerializer.Deserialize<MineField>(deseralization);
  }
}

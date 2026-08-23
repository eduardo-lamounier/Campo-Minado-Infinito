using System.Text.Json;
using CampoMinado.Core;

namespace CampoMinado.Saves;

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

  private readonly JsonSerializerOptions Options = new() { WriteIndented = false };

  private readonly string Filename = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "Campo-Minado-Infinito",
    "MineField.json"
  );

  /// <summary>
  /// Retorna a instância da classe.
  /// Caso não houver uma instância, cria-a, se tiver retorna a instância criada na primeira vez
  /// </summary>
  public static MineFieldSerializer GetInstance(MineField field)
  {
    if (s_instance == null)
    {
      s_instance = new MineFieldSerializer(field);
    }

    return s_instance;
  }

  /// <summary>
  /// Serializa o mapa, adicionando ele no arquivo JSON MineField.json
  /// </summary>
  /// <exception cref="InvalidOperationException">
  /// Lançada caso o serializador não tenha sido inicializado.
  /// </exception>
  public static void Serialize()
  {
    if (s_instance is null)
    {
      throw new InvalidOperationException("O serializador não foi inicializado.");
    }

    Directory.CreateDirectory(Path.GetDirectoryName(s_instance.Filename)!);

    var option = s_instance.Options;
    string serialization = JsonSerializer.Serialize(s_instance.Field, option);

    File.WriteAllText(s_instance.Filename, serialization);
  }

  /// <summary>
  /// Desserializa o campo do mapa e exclui ele do arquivo JSON
  /// </summary>
  /// <returns> Retorna o Campo do mapa que estava serializado, ou null se o arquivo não existir </returns>
  /// <exception cref="InvalidOperationException">
  /// Lançada caso o serializador não tenha sido inicializado.
  /// </exception>
  /// <exception cref="JsonException">
  /// Lançada quando o arquivo JSON está vazio.
  /// </exception>
  public static MineField? Deserialize()
  {
    if (s_instance is null)
    {
      throw new InvalidOperationException("O serializador não foi inicializado.");
    }

    if (!File.Exists(s_instance.Filename))
    {
      return null;
    }

    string? content = File.ReadAllText(s_instance.Filename);
    bool haveContent = !string.IsNullOrWhiteSpace(content);

    if (!haveContent)
    {
      throw new JsonException("O arquivo JSON está vazio. Não é possível desserializar.");
    }

    MineField? field = JsonSerializer.Deserialize<MineField>(content, s_instance.Options);

    if (field is not null)
      s_instance.Field = field;

    return field;
  }
}

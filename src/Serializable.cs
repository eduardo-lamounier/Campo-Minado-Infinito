using System.Text.Json;
using CampoMinado.Core;

namespace CampoMinado.src;

/// <summary>
/// Classe responsável pela serialização do mapa.
/// </summary>
public sealed class Serializable
{
  /// <summary>
  /// Privado para não haver perigo de haver duas instâncias
  /// </summary>
  private Serializable() { }

  /// <summary>
  /// Única instância da classe
  /// </summary>
  private static Serializable _instance;

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
    public static Serializable GetInstance(MineField field) 
    {
        if(_instance == null)
        {
            _instance = new Serializable() { Field = field };
        }

        return _instance;
    }

  /// <summary>
  /// Serializa o mapa, adicionando ele no arquivo JSON MineField.json
  /// </summary>
    public static void Serialize()
    {
        var option = _instance.Options;
        string serialization = JsonSerializer.Serialize(_instance.Field, option);

        File.WriteAllText(_instance.Filename, serialization);
    }
  
  /// <summary>
  /// Deserializa o campo do mapa
  /// </summary>
  /// <returns> Retorna o Campo do mapa que estava serializado </returns>
  public static MineField? Deserialize()
  {
    string deseralization = File.ReadAllText(_instance.Filename);

    return JsonSerializer.Deserialize<MineField>(deseralization);
  }
}

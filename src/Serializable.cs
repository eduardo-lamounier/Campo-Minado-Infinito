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

    public void Serialize(MineField field)
    {
        string filename = "MineField.json";
        string serialization = JsonSerializer.Serialize(field);

        File.WriteAllText(filename, serialization);
    }
}

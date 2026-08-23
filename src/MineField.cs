using CampoMinado.Core.Exceptions;
using System.Text.Json.Serialization;

namespace CampoMinado.Core;

/// <summary>
/// Representa o mapa do campo minado. Armazena o estado de todas as células inicializadas
/// do mapa, encapsulando todos os detalhes de implementação.
/// </summary>
public class MineField
{
  /// <summary>
  /// Versão da classe
  /// </summary> 
  /// <remarks> 
  /// Serve para diferenciar as classes na desserialização
  /// fazendo com que não haja conflito.
  /// </remarks>
  [JsonInclude]
  private readonly static string UIDVersion = "1.0";

  /// <summary>
  /// Quantidade padrão de bombas por célula.
  /// </summary>
  public const double DEFAULT_BOMB_DENSITY = 0.3;

  /// <summary>
  /// Quantidade de bomba por célula definida para o campo minado.
  /// </summary>
  public double BombDensity { get; }

  /// <summary>
  /// Gerencia o estado de uma região de 256 <see cref="Cell"/> (16x16).
  /// </summary>
  private class Chunk
  {
    private Cell[,] _cells = new Cell[16, 16];

    public ref Cell At(uint x, uint y) => ref _cells[y, x];

    public ref Cell At((uint X, uint Y) pos) => ref _cells[pos.Y, pos.X];

    // Inicializa o grid escolhendo posições aleatórias para conter bombas
    public Chunk(double bombDensity)
    {
      // Inicializa a chunk com bombas
      Random rand = new();
      for (uint bombsPlaced = 0; bombsPlaced < 16 * 16 * bombDensity;)
      {
        uint x = (uint)rand.Next(0, 16);
        uint y = (uint)rand.Next(0, 16);

        if (At(x, y) is not DangerousCell)
        {
          At(x, y) = new DangerousCell();
          bombsPlaced++;
        }
      }
    }
  }

  /// <summary>
  /// Armazena as chunks inicializadas - ou seja, aquelas na quão tiveram alguma célula
  /// interagindo com o jogador.
  /// </summary>
  [JsonInclude]
  private Dictionary<(int X, int Y), Chunk> _chunks = [];

  /// <summary>
  /// Armazena a quantidade de chunks geradas até então no campo minado.
  /// </summary>
  public int GeneratedChunksCount => _chunks.Count;

  /// <summary>
  /// Retorna a <see cref="Chunk"/> atribuída a uma coordenada. Se essa chunk não
  /// existir, ela é inicializada.
  /// </summary>
  private Chunk ChunkAt(int cx, int cy) => ChunkAt((cx, cy));

  /// <summary>
  /// Retorna a <see cref="Chunk"/> atribuída a uma coordenada. Se essa chunk não
  /// existir, ela é inicializada.
  /// </summary>
  private Chunk ChunkAt((int X, int Y) chunkPosition)
  {
    _chunks.TryAdd(chunkPosition, new Chunk(BombDensity));
    return _chunks[chunkPosition];
  }

  /// <summary>
  /// Retorna a posição atribuída à <see cref="Chunk"/> na quão a célula em uma coordenada
  /// específica pertence.
  /// </summary>
  private (int X, int Y) ChunkPositionFrom(int x, int y) =>
    ((int)Math.Floor((double)x / 16), (int)Math.Floor((double)y / 16));

  /// <summary>
  /// Retorna a posição atribuída à <see cref="Chunk"/> na quão a célula em uma coordenada
  /// específica pertence.
  /// </summary>
  private (int X, int Y) ChunkPositionFrom((int X, int Y) pos) =>
    ChunkPositionFrom(pos.X, pos.Y);

  /// <summary>
  /// Retorna a posição, da célula na coordenada específicada, dentro da <see cref="Chunk"/>
  /// na quão ela pertence.
  /// </summary>
  private (uint X, uint Y) InChunkPositionFrom(int x, int y) =>
    ((uint)((x % 16 + 16) % 16), (uint)((y % 16 + 16) % 16));

  /// <summary>
  /// Retorna a posição, da célula na coordenada específicada, dentro da <see cref="Chunk"/>
  /// na quão ela pertence.
  /// </summary>
  private (uint X, uint Y) InChunkPositionFrom((int X, int Y) pos) =>
    InChunkPositionFrom(pos.X, pos.Y);

  /// <summary>
  /// Retorna verdadeiro caso a célula especificada tenha bomba, ou falso caso contrário.
  /// </summary>
  /// <remarks>
  /// Diferentemente de <see cref="At"/>, não inicializa a célula.
  /// </remarks>
  public bool IsDangerousAt(int x, int y)
  {
    var chunkPosition = ChunkPositionFrom(x, y);
    var inChunkPosition = InChunkPositionFrom(x, y);

    var chunk = ChunkAt(chunkPosition);
    var cell = chunk.At(inChunkPosition);
    return cell is DangerousCell;
  }

  /// <summary>
  /// Retorna verdadeiro caso a célula especificada tenha bomba, ou falso caso contrário.
  /// </summary>
  /// <remarks>
  /// Diferentemente de <see cref="At"/>, não inicializa a célula.
  /// </remarks>
  public bool IsDangerousAt((int X, int Y) pos) => IsDangerousAt(pos.X, pos.Y);

  /// <summary>
  /// Retorna a célula em uma coordenada específica do mapa
  ///
  /// Garante que a célula vai existir, inicializando regiões
  /// do mapa automaticamente
  /// </summary>
  public Cell At(int x, int y)
  {
    (int X, int Y) chunkPosition = ChunkPositionFrom(x, y);
    (uint X, uint Y) inChunkPosition = InChunkPositionFrom(x, y);

    var chunk = ChunkAt(chunkPosition);
    ref var cell = ref chunk.At(inChunkPosition);

    if (cell is DangerousCell || cell is SafeCell)
      return cell;

    int bombsCount = 0;
    for (int dx = -1; dx <= 1; dx++)
      for (int dy = -1; dy <= 1; dy++)
        if ((dx != 0 || dy != 0) && IsDangerousAt(x + dx, y + dy))
          bombsCount++;

    cell = new SafeCell(bombsCount);
    return cell;
  }

  /// <summary>
  /// Retorna a célula em uma coordenada específica do mapa
  ///
  /// Garante que a célula vai existir, inicializando regiões
  /// do mapa automaticamente
  /// </summary>
  public Cell At((int X, int Y) pos) => At(pos.X, pos.Y);

  /// <summary>
  /// Retorna <see cref="true"/> caso a célula na posição especificada tenha
  /// sido inicializada. Retorna <see cref="false"/> caso contrário.
  /// </summary>
  public bool InitializedAt((int X, int Y) pos)
  {
    var chunkPosition = ChunkPositionFrom(pos);
    if (!_chunks.ContainsKey(chunkPosition))
      return false;

    var inChunkPosition = InChunkPositionFrom(pos);

    var chunk = _chunks[chunkPosition];
    var cell = chunk.At(inChunkPosition);

    return cell is DangerousCell || cell is SafeCell;
  }

  /// <summary>
  /// Retorna <see cref="true"/> caso a célula na posição especificada tenha
  /// sido inicializada. Retorna <see cref="false"/> caso contrário.
  /// </summary>
  public bool InitializedAt(int x, int y) => InitializedAt((x, y));

  /// <summary>
  /// Retorna uma matriz com as células em uma região quadrada do grid.
  /// </summary>
  /// <param name="positionX">
  /// Coordenada X do canto superior à esquerda da região.
  /// </param>
  /// <param name="positionY">
  /// Coordenada Y do canto superior à esquerda da região.
  /// </param>
  /// <param name="width">
  /// Largura (em quant. de células) da região. A região se extenderá para a direita.
  /// </param>
  /// <param name="height">
  /// Altura (em quant. de células) da região. A região se extenderá para baixo.
  /// </param>
  /// <remarks>
  /// Matematicamente:
  /// <list>
  /// <item><description>
  ///   as coordenadas X da região estão no intervalo `[positionX, positionX+width[`
  /// </description></item>
  /// <item><description>
  ///   as coordenadas Y da região estão no intervalo `[positionY,positionY-height[`
  /// </description></item>
  /// </list>
  /// </remarks>
  public Cell[,] GetRegion(int positionX, int positionY, uint width, uint height)
  {
    var region = new Cell[width, height];

    for (int i = 0; i < height; i++)
      for (int j = 0; j < width; j++)
        region[i, j] = At(positionX + i, positionY - j);

    return region;
  }

  /// <summary>
  /// Retorna uma matriz com as células em uma região quadrada do grid.
  /// </summary>
  /// <param name="position">
  /// Coordenada do canto superior à esquerda da região.
  /// </param>
  /// <param name="width">
  /// Largura (em quant. de células) da região. A região se extenderá para a direita.
  /// </param>
  /// <param name="height">
  /// Altura (em quant. de células) da região. A região se extenderá para baixo.
  /// </param>
  /// <remarks>
  /// Matematicamente:
  /// <list>
  /// <item><description>
  ///   as coordenadas X da região estão no intervalo `[position.X, position.X+width[`
  /// </description></item>
  /// <item><description>
  ///   as coordenadas Y da região estão no intervalo `[position.Y,position.Y-height[`
  /// </description></item>
  /// </list>
  /// </remarks>
  public Cell[,] GetRegion((int X, int Y) position, uint width, uint height) =>
    GetRegion(position.X, position.Y, width, height);

  /// <param name="bombDensity">
  /// Densidade de bomba deve estar no intervalo [0, 1].
  /// </param>
  /// <exception cref="InvalidBombDensityException">
  /// Lançada se a densidade estiver fora de [0, 1].
  /// </exception>
  public MineField(double bombDensity = DEFAULT_BOMB_DENSITY)
  {
    if (bombDensity < 0.0 || bombDensity > 1.0)
      throw new InvalidBombDensityException(bombDensity);

    BombDensity = bombDensity;
  }
}

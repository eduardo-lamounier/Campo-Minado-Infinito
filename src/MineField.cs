namespace CampoMinado.Core;

public class MineField
{
  public const double BOMBS_DENSITY = 0.1;

  private class Chunk
  {
    private Cell[,] _cells = new Cell[16, 16];

    public Cell At(uint x, uint y) => _cells[y, x];

    public Cell At((uint X, uint Y) pos) => _cells[pos.Y, pos.X];

    // Inicializa o grid escolhendo posições aleatórias para conter bombas
    public Chunk()
    {
      // Inicializa a chunk com bombas
      Random rand = new();
      for (uint bombsPlaced = 0; bombsPlaced < 16 * 16 * MineField.BOMBS_DENSITY;)
      {
        uint x = (uint)rand.Next(0, 16);
        uint y = (uint)rand.Next(0, 16);

        if (!At(x, y).HaveBomb)
        {
          At(x, y).PlaceBomb();
          bombsPlaced++;
        }
      }
    }
  }

  private Dictionary<(int X, int Y), Chunk> _chunks = [];

  /// <summary>
  /// Retorna a chunk atribuída a uma coordenada. Se essa chunk não
  /// existir, ela é inicializada.
  /// </summary>
  private Chunk ChunkAt(int cx, int cy) => ChunkAt((cx, cy));

  /// <summary>
  /// Retorna a chunk atribuída a uma coordenada. Se essa chunk não
  /// existir, ela é inicializada.
  /// </summary>
  private Chunk ChunkAt((int X, int Y) chunkPosition)
  {
    _chunks.TryAdd(chunkPosition, new Chunk());
    return _chunks[chunkPosition];
  }

  private record Positions2DPair((int X, int Y) chunkPos, (int X, int Y) inChunkPos);

  private (int X, int Y) ChunkPositionFrom(int x, int y) =>
    ((int)Math.Floor((double)x / 16), (int)Math.Floor((double)y / 16));

  private (int X, int Y) ChunkPositionFrom((int X, int Y) pos) =>
    ChunkPositionFrom(pos.X, pos.Y);

  private (uint X, uint Y) InChunkPositionFrom(int x, int y) =>
    ((uint)((x % 16 + 16) % 16), (uint)((y % 16 + 16) % 16));

  private (uint X, uint Y) InChunkPositionFrom((int X, int Y) pos) =>
    InChunkPositionFrom(pos.X, pos.Y);

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
    var cell = chunk.At(inChunkPosition);

    if (cell.GotInitialized)
      return cell;

    int bombsCount = 0;
    for (int dx = -1; dx <= 1; dx++)
      for (int dy = -1; dy <= 1; dy++)
        if ((dx != 0 && dy != 0) && At(x + dx, y + dy).HaveBomb)
          bombsCount++;

    cell.NearBombs = bombsCount;
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
  /// Retorna verdadeiro (`true`) caso a célula na posição especificada tenha
  /// sido inicializada. Retorna falso (`false`) caso contrário.
  /// </summary>
  public bool InitializedAt((int X, int Y) pos)
  {
    var chunkPosition = ChunkPositionFrom(pos);
    if (!_chunks.ContainsKey(chunkPosition)) return false;

    var inChunkPosition = InChunkPositionFrom(pos);

    var chunk = _chunks[chunkPosition];
    var cell = chunk.At(inChunkPosition);

    return cell.GotInitialized;
  }

  /// <summary>
  /// Retorna verdadeiro (`true`) caso a célula na posição especificada tenha
  /// sido inicializada. Retorna falso (`false`) caso contrário.
  /// </summary>
  public bool InitializedAt(int x, int y) => InitializedAt((x, y));

  public MineField() { }
}

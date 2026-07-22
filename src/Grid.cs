namespace CampoMinado.Grid;

public class Grid
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
      for (uint bombsPlaced = 0; bombsPlaced < 16 * 16 * Grid.BOMBS_DENSITY;)
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

  private Chunk _origin = new();
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

  /// <summary>
  /// Retorna a célula em uma coordenada específica do mapa
  ///
  /// Garante que a célula vai existir, inicializando regiões
  /// do mapa automaticamente
  /// </summary>
  public Cell At(int x, int y)
  {
    (int X, int Y) chunkPosition = (
      (int)Math.Floor((double)x / 16),
      (int)Math.Floor((double)y / 16)
    );

    (uint X, uint Y) inChunkPosition = (
      (uint)(x - chunkPosition.X * 16),
      (uint)(y - chunkPosition.Y * 16)
    );

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

  public Grid()
  {
    _chunks[(0, 0)] = _origin;
  }
}

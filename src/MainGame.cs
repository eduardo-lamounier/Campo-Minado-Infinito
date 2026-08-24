using CampoMinado.Core;
using CampoMinado.Rendering;
using CampoMinado.Saves;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CampoMinado;

/// <summary>
/// Classe principal do programa. Contém a definição do método de entrada e a lógica do game loop.
/// </summary>
public class MainGame : Game
{
  public static void Main() => new MainGame("Campo Minado Infinito", 1280, 720).Run();

  /// <summary>
  /// Armazena o estado do campo minado da sessão de jogo atual
  /// </summary>
  public MineField Field { get; private set; }

  /// <summary>
  /// Responsável por renderizar as células do <see cref="MineField"/> na tela.
  /// </summary>
  private MineFieldRenderer FieldRenderer { get; set; }

  /// <summary>
  /// Gerencia as configurações relacionadas aos gráficos e à janela do programa.
  /// </summary>
  public GraphicsDeviceManager Graphics { get; private set; }

  /// <summary>
  /// Representa o dispositivo responsável pela renderização dos gráficos do programa.
  /// </summary>
  public new GraphicsDevice GraphicsDevice { get; private set; }

  /// <summary>
  /// Responsável pela renderização de sprites e texturas na tela.
  ///
  /// Necessário para <see cref="Sprite.Draw"/>.
  /// </summary>
  public SpriteBatch SpriteBatch { get; private set; }

  /// <summary>
  /// Gerencia o carregamento e o acesso aos assets do programa.
  /// </summary>
  public new ContentManager Content { get; private set; }

  /// <summary>
  /// Armazena o início da região retangular atualmente sendo exibida na tela do
  /// jogador.
  /// </summary>
  private (int X, int Y) _currentPosition = (0, 0);

  private const uint _baseSpriteSize = 16; // Todo sprite é 16x16 pixels
  private const float _spriteScaling = 2;

  /// <summary>
  /// Retorna o tamanho dos sprites da célula levando em consideração o "zoom" atual.
  /// </summary>
  private static uint SpriteSize => (uint)(_baseSpriteSize * _spriteScaling);

  private KeyboardState _previousKeyboardState = new();
  private MouseState _previousMouseState = new();

  public bool GameOver { get; private set; }

  /// <summary>
  /// Revela recursivamente a região de células vazias adjacentes a partir de
  /// (<paramref name="x"/>, <paramref name="y"/>).
  /// </summary>
  /// <remarks>
  /// Chamado quando a célula clicada não possui bombas adjacentes
  /// (<see cref="SafeCell.NearBombs"/> == 0).
  ///
  /// Revela a célula inicial e, para célula adjacente, continua expandindo
  /// se ela também for vazia e ainda não estiver revelada (flood fill);
  /// caso contrário, apenas revela a célula sem expandir — células numeradas
  /// na borda da região.
  /// </remarks>
  private void RevealEmpty(int x, int y)
  {
    Field.At(x, y).Reveal();

    for (int dx = -1; dx <= 1; dx++)
      for (int dy = -1; dy <= 1; dy++)
      {
        var cell = (SafeCell)Field.At(x + dx, y + dy);

        if (cell.NearBombs == 0 && !cell.IsRevealed)
          RevealEmpty(x + dx, y + dy);
        else
          cell.Reveal();
      }
  }

  /// <summary>
  /// Revela todas as células da região atual
  /// </summary>
  private void RevealAll()
  {
    Cell[,] region = Field.GetRegion(
      _currentPosition,
      (uint)Window.ClientBounds.Width / SpriteSize,
      (uint)Window.ClientBounds.Height
    );

    foreach (var c in region)
      if (!c.HasFlag)
        c.Reveal();
  }

  /// <summary>
  /// Método chamado a todo frame. Contém lógica do programa que tem que ser
  /// atualizada a todo momento.
  /// </summary>
  /// <remarks>
  /// NÃO é responsável pela lógica de renderização, mas chama automaticamente o
  /// método <see cref="Draw"/>, que tem.
  /// </remarks>
  protected override void Update(GameTime gameTime)
  {
    if (GameOver)
    {
      var gameOverKeyboard = Keyboard.GetState();
      foreach (var key in gameOverKeyboard.GetPressedKeys())
      {
        if (_previousKeyboardState.IsKeyUp(key))
        {
          Exit();
          return;
        }
      }
      _previousKeyboardState = gameOverKeyboard;
      return;
    }

    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
      Exit();

    if (Keyboard.GetState().IsKeyDown(Keys.F4) && _previousKeyboardState.IsKeyUp(Keys.F4))
      MineFieldSerializer.Serialize();

    if (Keyboard.GetState().IsKeyDown(Keys.F5) && _previousKeyboardState.IsKeyUp(Keys.F5))
    {
      var field = MineFieldSerializer.Deserialize();
      if (field is not null)
        Field = field;
    }

    Console.Clear();

    Console.WriteLine("Tempo passado: " + gameTime.TotalGameTime);

    var fps = TimeSpan.FromSeconds(1) / gameTime.ElapsedGameTime;
    Console.WriteLine("FPS (aproximado): " + fps);

    Console.WriteLine("Chunks geradas: " + Field.GeneratedChunksCount);

    // Movimentação do mapa:

    var keyboard = Keyboard.GetState();

    if (keyboard.IsKeyDown(Keys.W) && _previousKeyboardState.IsKeyUp(Keys.W))
    {
      _currentPosition.Y += (int)(Window.ClientBounds.Height / SpriteSize);
    }
    else if (keyboard.IsKeyDown(Keys.A) && _previousKeyboardState.IsKeyUp(Keys.A))
    {
      _currentPosition.X -= (int)(Window.ClientBounds.Width / SpriteSize);
    }
    else if (keyboard.IsKeyDown(Keys.S) && _previousKeyboardState.IsKeyUp(Keys.S))
    {
      _currentPosition.Y -= (int)(Window.ClientBounds.Height / SpriteSize);
    }
    else if (keyboard.IsKeyDown(Keys.D) && _previousKeyboardState.IsKeyUp(Keys.D))
    {
      _currentPosition.X += (int)(Window.ClientBounds.Width / SpriteSize);
    }

    _previousKeyboardState = keyboard;

    Console.WriteLine(
      "Posição atual (x, y): " + _currentPosition.X + ", " + _currentPosition.Y
    );

    // Interação com o mouse:

    MouseState mouse = Mouse.GetState();

    int x = _currentPosition.X + mouse.X / (int)SpriteSize;
    int y = _currentPosition.Y - mouse.Y / (int)SpriteSize;

    if (
      mouse.LeftButton == ButtonState.Pressed
      && _previousMouseState.LeftButton == ButtonState.Released
    )
    {
      Cell cell = Field.At(x, y);

      // Game over:
      if (!cell.HasFlag && cell is DangerousCell)
      {
        GameOver = true;
        RevealAll();
        Console.WriteLine("GAME OVER!");
        return;
      }

      if (!cell.IsRevealed && !cell.HasFlag)
      {
        var safeCell = (SafeCell)cell;
        if (safeCell.NearBombs == 0)
          RevealEmpty(x, y);
        else
          safeCell.Reveal();
      }
    }

    if (
      mouse.RightButton == ButtonState.Pressed
      && _previousMouseState.RightButton == ButtonState.Released
    )
    {
      Cell cell = Field.At(x, y);

      if (!cell.IsRevealed)
      {
        if (cell.HasFlag)
        {
          cell.EnsureNoFlag();
        }
        else
        {
          cell.EnsureFlag();
        }
      }
    }

    _previousMouseState = mouse;

    base.Update(gameTime);
  }

  /// <summary>
  /// Método chamado a todo frame. Contém lógica de renderização do programa.
  /// </summary>
  /// <remarks>
  /// NÃO é responsável pela lógica periódica genérica do programa, que é responsábilidade
  /// de <see cref="Update"/>.
  /// </remarks>
  protected override void Draw(GameTime gameTime)
  {
    GraphicsDevice.Clear(Color.CornflowerBlue);

    uint width = (uint)Window.ClientBounds.Width; // Largura da janela
    uint height = (uint)Window.ClientBounds.Height; // Altura da janela

    uint m = height / SpriteSize; // Quant. linhas da região
    uint n = width / SpriteSize; // Quant. colunas da região

    // Região do campo minado a ser desenhada na tela:
    Cell[,] region = Field.GetRegion(_currentPosition.X, _currentPosition.Y, n, m);

    SpriteBatch.Begin(); // Começa a desenhar na tela

    for (int i = 0; i < m; i++)
    {
      for (int j = 0; j < n; j++)
      {
        Sprite sprite = FieldRenderer
          .RenderCell(region[i, j])
          .WithScale(_spriteScaling)
          .WithPosition(SpriteSize * j, SpriteSize * i);
        sprite.Draw(SpriteBatch);
      }
    }

    SpriteBatch.End(); // Termina de desenhar na tela

    // Sprite spriteCelula = FieldRenderer.RenderCell(region[i,j]);
    // spriteCelula.Draw(SpriteBatch);

    base.Draw(gameTime);
  }

  /// <summary>
  /// Contém a lógica de inicialização do programa. É chamada antes do game loop com
  /// <see cref="Update"/> e <see cref="Draw"/>.
  /// </summary>
  /// <remarks>
  /// NÃO é responsável pela lógica de inicialização da classe nem pela lógica de
  /// carregamento dos sprites, texturas etc (assets em geral).
  ///
  /// Chama <see cref="LoadContent"/> - que é responsável pelo carregamento de assets -
  /// automaticamente.
  /// </remarks>
  protected override void Initialize()
  {
    // TODO: Carregar o campo minado do arquivo caso ele exista.

    base.Initialize();
  }

  /// <summary>
  /// Contém a lógica de carregamento dos assets do programa. É chamada antes do game
  /// loop, depois da lógica de inicialização genérica - <see cref="Initialize"/>.
  /// </summary>
  protected override void LoadContent() { }

  /// <summary>
  /// Inicializa o objeto <see cref="MainGame"/>, mas não inicia o jogo em si: para isso é necessário
  /// chamar o método <see cref="Game.Run"/>.
  /// </summary>
  /// <param name="title">Recebe o título da janela do programa.</param>
  /// <param name="width">
  ///   Recebe a quantidade de pixels de largura da janela do programa.
  /// </param>
  /// <param name="height">
  ///   Recebe a quantidade de pixels de altura da janela do programa.
  /// </param>
  public MainGame(string title, int width, int height)
  {
    Graphics = new(this)
    {
      PreferredBackBufferWidth = width,
      PreferredBackBufferHeight = height,
    };

    Graphics.ApplyChanges();

    Window.Title = title;

    Content = base.Content;
    Content.RootDirectory = "Content";

    IsMouseVisible = true;

    GraphicsDevice = base.GraphicsDevice;
    SpriteBatch = new SpriteBatch(GraphicsDevice);
    Field = new(0.15);
    FieldRenderer = new(Content);
    MineFieldSerializer.GetInstance(Field);
  }
}

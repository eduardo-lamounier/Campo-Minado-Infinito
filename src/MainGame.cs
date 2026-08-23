using CampoMinado.Core;
using CampoMinado.Rendering;
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

  private MineField Field { get; set; }

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
  /// Método chamado a todo frame. Contém lógica do programa que tem que ser
  /// atualizada a todo momento.
  /// </summary>
  /// <remarks>
  /// NÃO é responsável pela lógica de renderização, mas chama automaticamente o
  /// método <see cref="Draw"/>, que tem.
  /// </remarks>
  protected override void Update(GameTime gameTime)
  {
    if (
      GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
      || Keyboard.GetState().IsKeyDown(Keys.Escape)
    )
      Exit();

    /* TODO: Checar pelos atalhos:
     * - F5: Salvar o estado do campo minado no arquivo de salvamento.
     * - F4: Reiniciar o campo minado.
     */

    Console.Clear();
    Console.WriteLine("Tempo passado: " + gameTime.TotalGameTime);
    var fps = TimeSpan.FromSeconds(1) / gameTime.ElapsedGameTime;
    Console.WriteLine("FPS (aproximado): " + fps);

    // TODO: Implementar lógica de jogo: revelação de células, posicionamento de bandeiras
    // etc

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

    // TODO: Desenhar o campo minado carregado

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
    Graphics = new(this);
    Graphics.PreferredBackBufferWidth = width;
    Graphics.PreferredBackBufferHeight = height;

    Graphics.ApplyChanges();

    Window.Title = title;

    Content = base.Content;
    Content.RootDirectory = "Content";

    IsMouseVisible = true;

    GraphicsDevice = base.GraphicsDevice;
    SpriteBatch = new SpriteBatch(GraphicsDevice);
    Field = new();
  }
}

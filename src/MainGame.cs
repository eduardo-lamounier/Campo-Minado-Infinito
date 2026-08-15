using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class MainGame : Game
{
  public static void Main() => new MainGame("Campo Minado Infinito", 1280, 720).Run();

  public GraphicsDeviceManager Graphics { get; private set; }

  public new GraphicsDevice GraphicsDevice { get; private set; }

  public SpriteBatch SpriteBatch { get; private set; }

  public new ContentManager Content { get; private set; }

  protected override void Update(GameTime gameTime)
  {
    if (
      GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
      || Keyboard.GetState().IsKeyDown(Keys.Escape)
    )
      Exit();

    Console.Clear();
    Console.WriteLine("Tempo passado: " + gameTime.TotalGameTime);
    var fps = TimeSpan.FromSeconds(1) / gameTime.ElapsedGameTime;
    Console.WriteLine("FPS (aproximado): " + fps);

    base.Update(gameTime);
  }

  protected override void Draw(GameTime gameTime)
  {
    GraphicsDevice.Clear(Color.CornflowerBlue);

    base.Draw(gameTime);
  }

  protected override void Initialize()
  {
    base.Initialize();

    GraphicsDevice = base.GraphicsDevice;
    SpriteBatch = new SpriteBatch(GraphicsDevice);
  }

  protected override void LoadContent() { }

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
  }
}

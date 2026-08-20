using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CampoMinado.Rendering;

public record class Sprite(Texture2D texture)
{
  public Texture2D Texture { get; } = texture;

  public Vector2 Position { get; private set; } = Vector2.Zero;

  public Rectangle? SourceRect { get; private set; }

  public float Rotation { get; private set; } = 0;

  public Color Color { get; private set; } = Color.White;

  public Vector2 Origin { get; private set; } = Vector2.Zero;

  public Vector2 Scale { get; private set; } = Vector2.One;

  public SpriteEffects Effects { get; private set; } = SpriteEffects.None;

  public float LayerDepth { get; private set; } = 0;

  public Sprite WithPosition(Vector2 position) => this with { Position = position };

  public Sprite WithPosition(float x, float y) => this with { Position = new(x, y) };

  public Sprite WithSourceRectangle(Rectangle sourceRect) =>
    this with
    {
      SourceRect = sourceRect,
    };

  public Sprite WithRotation(float rotation) => this with { Rotation = rotation };

  public Sprite WithColor(Color color) => this with { Color = color };

  public Sprite WithOrigin(Vector2 origin) => this with { Origin = origin };

  public Sprite WithOrigin(float x, float y) => this with { Position = new(x, y) };

  public Sprite WithScale(Vector2 scale) => this with { Scale = scale };

  public Sprite WithScale(float scale) => this with { Scale = new(scale, scale) };

  public Sprite WithEffects(SpriteEffects effects) => this with { Effects = effects };

  public Sprite WithLayerDepth(float layerDepth) => this with { LayerDepth = layerDepth };

  public void Draw(SpriteBatch spriteBatch)
  {
    spriteBatch.Draw(
      Texture,
      Position,
      SourceRect,
      Color,
      Rotation,
      Origin,
      Scale,
      Effects,
      LayerDepth
    );
  }
}

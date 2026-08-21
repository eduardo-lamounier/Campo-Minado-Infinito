using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CampoMinado.Rendering;

/// <summary>
/// Armazena informações configuráveis e necessárias para desenhar um sprite na tela.
///
/// Permite que classes/métodos de renderização retornem um sprite pronto
/// (ou parcialmente) para ser desenhado.
/// </summary>
public record class Sprite(Texture2D texture)
{
  /// <summary>
  /// Textura do sprite a ser desenhado
  /// </summary>
  public Texture2D Texture { get; } = texture;

  /// <summary>
  /// Posição do sprite na tela do programa.
  /// </summary>
  public Vector2 Position { get; private set; } = Vector2.Zero;

  /// <summary>
  /// Área retangular da textura escolhida, em pixels.
  /// </summary>
  /// <remarks>
  /// Quando `null`, toda a textura será renderizada.
  /// </remarks>
  public Rectangle? SourceRect { get; private set; }

  /// <summary>
  /// Rotação, em radianos, do sprite (sentido anti-horário).
  /// </summary>
  public float Rotation { get; private set; } = 0;

  /// <summary>
  /// Colorização do sprite - essa cor é multiplicada pelas cores que fazem parte da
  /// textura do sprite.
  /// </summary>
  public Color Color { get; private set; } = Color.White;

  /// <summary>
  /// Posição/Offset do sprite em relação ao topo esquerdo.
  ///
  /// Define onde 'Position' realmente apontará na tela, como o sprite será
  /// rotacionado e como ele é escalado.
  /// </summary>
  public Vector2 Origin { get; private set; } = Vector2.Zero;

  /// <summary>
  /// Escala do sprite. Os valores 'X' e 'Y' são multiplicados pela largura e altura do
  /// sprite.
  /// </summary>
  /// <remarks>
  /// Pode distorcer a imagem de forma indesejada caso as componentes 'X' e 'Y' sejam
  /// diferentes. Necessário ter cuidado.
  /// </remarks>
  public Vector2 Scale { get; private set; } = Vector2.One;

  /// <summary>
  /// Define se o sprite vai ser espelhado de alguma forma.
  /// </summary>
  public SpriteEffects Effects { get; private set; } = SpriteEffects.None;

  /// <summary>
  /// Dependendo do modo de renderização do 'SpriteBatch' passado na hora de desenhar o
  /// sprite, define quais sprites ficam "a frente" de outros na tela.
  /// </summary>
  /// <remarks>
  /// Não tem efeito quando 'SpriteBatch.Begin(...)' é chamado, por exemplo, com
  /// 'sortMode: SpriteSortMode.Deferred' - padrão caso nada seja passado.
  /// </remarks>
  public float LayerDepth { get; private set; } = 0;

  /// <summary>
  /// Retorna o sprite com a posição alterada.
  /// </summary>
  public Sprite WithPosition(Vector2 position) => this with { Position = position };

  /// <summary>
  /// Retorna o sprite com a posição alterada.
  /// </summary>
  public Sprite WithPosition(float x, float y) => this with { Position = new(x, y) };

  /// <summary>
  /// Retorna o sprite com a região retangular da textura alterada.
  /// </summary>
  public Sprite WithSourceRectangle(Rectangle sourceRect) =>
    this with
    {
      SourceRect = sourceRect,
    };

  /// <summary>
  /// Retorna o sprite com toda a textura sendo renderizada - nenhuma área específica.
  /// </summary>
  public Sprite WithNoSourceRectangle() => this with { SourceRect = null };

  /// <summary>
  /// Retorna o sprite com a rotação alterada.
  /// </summary>
  public Sprite WithRotation(float rotation) => this with { Rotation = rotation };

  /// <summary>
  /// Retorna o sprite com a cor alterada.
  /// </summary>
  public Sprite WithColor(Color color) => this with { Color = color };

  /// <summary>
  /// Retorna o sprite com a posição de origem alterada.
  /// </summary>
  public Sprite WithOrigin(Vector2 origin) => this with { Origin = origin };

  /// <summary>
  /// Retorna o sprite com a posição de origem alterada.
  /// </summary>
  public Sprite WithOrigin(float x, float y) => this with { Position = new(x, y) };

  /// <summary>
  /// Retorna o sprite com a escala alterada.
  /// </summary>
  /// <param name="scale">
  ///   Escala das dimensões do sprite. A componente 'X' é aplicada sobre a largura do
  ///   sprite, enquanto a componente 'Y' é aplicada sobre a altura.
  /// </param>
  public Sprite WithScale(Vector2 scale) => this with { Scale = scale };

  /// <summary>
  /// Retorna o sprite com a escala alterada.
  /// </summary>
  /// <param name="scale">
  ///   Escala para todas as dimensões do sprite.
  /// </param>
  public Sprite WithScale(float scale) => this with { Scale = new(scale, scale) };

  /// <summary>
  /// Retorna o sprite com efeito de espelhagem alterado.
  /// </summary>
  public Sprite WithEffects(SpriteEffects effects) => this with { Effects = effects };

  /// <summary>
  /// Retorna o sprite com profundidade de camada alterada.
  /// </summary>
  public Sprite WithLayerDepth(float layerDepth) => this with { LayerDepth = layerDepth };

  /// <summary>
  /// Desenha o sprite na tela usando o 'spriteBatch' específicado.
  /// </summary>
  /// <remarks>
  /// 'spriteBatch.Begin()' DEVE ter sido chamado antes desse método.
  /// </remarks>
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

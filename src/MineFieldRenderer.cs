using System.Diagnostics;
using CampoMinado.Core;
using CampoMinado.Core.Exceptions;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CampoMinado.Rendering;

/// <summary>
/// Responsável por renderizar células do campo minado, gerando os sprites que vão ser
/// desenhados na tela.
/// </summary>
public class MineFieldRenderer(ContentManager content)
{
  private ContentManager Content => content;

  private Texture2D TextureForCell(Cell cell)
  {
    Debug.Assert(Content.RootDirectory == "Content");

    if (cell is not SafeCell && cell is not DangerousCell)
      return Content.Load<Texture2D>("images/Cell_Unrevealed");

    if (!cell.IsRevealed)
    {
      if (cell.HasFlag)
        return Content.Load<Texture2D>("images/Cell_Unrevealed_WithFlag");

      return Content.Load<Texture2D>("images/Cell_Unrevealed");
    }

    if (cell is DangerousCell)
      return Content.Load<Texture2D>("images/Cell_Bomb");

    var nearBombs = ((SafeCell)cell).NearBombs;
    return Content.Load<Texture2D>($"images/Cell_{nearBombs}");
  }

  /// <summary>
  /// Retorna o sprite com textura correspondente à célula passada.
  ///
  /// Todas os outros atributos do sprite são definidos como padrão, logo pode ser
  /// necessário usar 'WithPosition', 'WithRotation' etc.
  /// </summary>
  public Sprite RenderCell(Cell cell)
  {
    var texture = TextureForCell(cell);
    return new Sprite(texture);
  }
}

using System.Diagnostics;
using CampoMinado.Core;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CampoMinado.Rendering;

/// <summary>
/// Responsável por renderizar células do campo minado, gerando os sprites que vão ser
/// desenhados na tela.
/// </summary>
public class MineFieldRenderer(ContentManager content)
{
  /// <summary>
  /// Gerencia o carregamento e o acesso aos assets do programa.
  /// </summary>
  private ContentManager Content => content;

  /// <summary>
  /// Retorna a textura correspondente ao estado de uma célula.
  /// </summary>
  private Texture2D TextureForCell(Cell cell)
  {
    Debug.Assert(Content.RootDirectory == "Content");

    // Célula não-inicializada:
    if (cell is not SafeCell && cell is not DangerousCell)
      return Content.Load<Texture2D>("images/Cell_Unrevealed");

    if (!cell.IsRevealed)
    {
      if (cell.HasFlag)
        return Content.Load<Texture2D>("images/Cell_Unrevealed_WithFlag");

      return Content.Load<Texture2D>("images/Cell_Unrevealed");
    }

    if (cell is SafeCell safeCell)
    {
      var nearBombs = safeCell.NearBombs;
      return Content.Load<Texture2D>($"images/Cell_{nearBombs}");
    }

    return Content.Load<Texture2D>("images/Cell_Bomb");
  }

  /// <summary>
  /// Retorna o sprite com textura correspondente ao estado da célula passada.
  ///
  /// Todos os outros atributos do sprite são definidos como padrão, logo pode ser
  /// necessário usar 'WithPosition', 'WithRotation' etc.
  /// </summary>
  public Sprite RenderCell(Cell cell)
  {
    var texture = TextureForCell(cell);
    return new Sprite(texture);
  }
}

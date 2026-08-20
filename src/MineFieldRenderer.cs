using System.Diagnostics;
using CampoMinado.Core;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CampoMinado.Rendering;

public class MineFieldRenderer(ContentManager content)
{
  private ContentManager Content => content;

  public Texture2D TextureForCell(Cell cell)
  {
    Debug.Assert(Content.RootDirectory == "Content");

    if (cell is not SafeCell && cell is not DangerousCell)
      return Content.Load<Texture2D>("images_Cell_Unrevealed");

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

  public Sprite RenderCell(Cell cell)
  {
    var texture = TextureForCell(cell);
    return new Sprite(texture);
  }
}

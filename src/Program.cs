using Spectre.Console;

namespace CampoMinado;

class Program
{
  static void Main(String[] args)
  {
    // Código base apenas para ter um "blueprint" de como vamos desenhar o
    // campo minado na tela e como vamos pegar o input do usuário

    var field = new Spectre.Console.Grid();

    int n = 10;

    for (int i = 0; i < n; i++)
    {
      field.AddColumn();
    }

    for (int i = 0; i < n; i++)
    {
      string[] columns = new string[n];

      for (int j = 0; j < n; j++)
      {
        columns[j] = $"{j}";
      }

      field.AddRow(columns);
    }

    AnsiConsole.Write(field);

    ConsoleKey? key = null;

    /*
    * Além do Console.BufferWidth e Console.BufferHeight, também tem
    * o Console.WindowWidth e Console.WindowHeight, que são diferentes
    * mas parecidos
    */

    while (key != ConsoleKey.Q)
    {
      int currentCol = Console.CursorLeft;
      int currentRow = Console.CursorTop;

      key = Console.ReadKey().Key;

      if (key == ConsoleKey.RightArrow)
      {
        if (currentCol == Console.BufferWidth)
        {
          currentCol = 0;
          Console.WriteLine($"Tela movida para a direita.");
        }
        else
          currentCol++;
      }
      else if (key == ConsoleKey.LeftArrow)
      {
        if (currentCol == 0)
        {
          currentCol = Console.BufferWidth;
          Console.WriteLine($"Tela movida para a esquerda.");
        }
        else
          currentCol--;
      }
      else if (key == ConsoleKey.DownArrow)
      {
        if (currentRow == Console.BufferHeight)
        {
          currentRow = 0;
          Console.WriteLine($"Tela movida para baixo.");
        }
        else
          currentRow++;
      }
      else if (key == ConsoleKey.UpArrow)
      {
        if (currentRow == 0)
        {
          currentRow = Console.BufferHeight;
          Console.WriteLine($"Tela movida para cima.");
        }
        else
          currentRow--;
      }

      Console.SetCursorPosition(currentCol, currentRow);
    }

    Console.WriteLine("Programa finalizado.");
  }
}

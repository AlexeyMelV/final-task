using System;
using System.Diagnostics;
using System.Drawing;
using System.Xml.Linq;
using static System.Console;

namespace SnakeGame
{
  internal class Program
  {
    private const int SizeVertical = 40;
    private const int SizeHorizontal = 40;

    private const int FrameMilliseconds = 100;

    private const ConsoleColor PerimetrColor = ConsoleColor.Red;

    private const ConsoleColor FoodColor = ConsoleColor.Green;

    private const ConsoleColor BodyColor = ConsoleColor.Cyan;
    private const ConsoleColor HeadColor = ConsoleColor.DarkBlue;

    private static readonly Random Random = new Random();

    static void Main()
    {
      SetWindowSize(SizeVertical, SizeHorizontal);
      SetBufferSize(SizeVertical, SizeHorizontal);
      CursorVisible = false;


      while (true)
      {
        Game();
        Thread.Sleep(2000);
        ReadKey();
      }
    }

    static void Game()
    {
      int score = 0;
      Clear();
      PrintPerimetr();
      Snake snake = new Snake(10, 5, HeadColor, BodyColor);

      Point food = GenFood(snake);
      food.Print();

      Direce currentMovement = Direce.Right;

      int lagMs = 0;
      var sw = new Stopwatch();

      while (true)
      {
        sw.Restart();
        Direce oldMovement = currentMovement;

        while (sw.ElapsedMilliseconds <= FrameMilliseconds - lagMs)
        {
          if (currentMovement == oldMovement)
            currentMovement = ReadMovement(currentMovement);
        }

        sw.Restart();

        if (snake.Head.X == food.X && snake.Head.Y == food.Y)
        {
          snake.Move(currentMovement, true);
          food = GenFood(snake);
          food.Print();

          score++;

          Task.Run(() => Beep(1200, 200));
        }
        else
        {
          snake.Move(currentMovement);
        }

        if (snake.Head.X == SizeVertical - 1
            || snake.Head.X == 0
            || snake.Head.Y == SizeHorizontal - 1
            || snake.Head.Y == 0
            || snake.Body.Any(b => b.X == snake.Head.X && b.Y == snake.Head.Y))
          break;

        lagMs = (int)sw.ElapsedMilliseconds;
      }

      snake.Clear();
      food.Clear();

      SetCursorPosition(SizeVertical / 3, SizeHorizontal / 3);
      WriteLine($"Game over, Score: {score}");

      Task.Run(() => Beep(200, 600));
    }

    static Point GenFood(Snake snake)
    {
      Point food;

      do
      {
        food = new Point(Random.Next(1, SizeVertical - 2), Random.Next(1, SizeHorizontal - 2), FoodColor);
      }
      while (snake.Head.X == food.X && snake.Head.Y == food.Y ||
               snake.Body.Any(b => b.X == food.X && b.Y == food.Y));
      return food;
    }

    static Direce ReadMovement(Direce currentDirece)
    {
      if (!KeyAvailable)
        return currentDirece;

      ConsoleKey key = ReadKey(true).Key;

      currentDirece = key switch
      {
        ConsoleKey.UpArrow when currentDirece != Direce.Down => Direce.Up,
        ConsoleKey.DownArrow when currentDirece != Direce.Up => Direce.Down,
        ConsoleKey.LeftArrow when currentDirece != Direce.Right => Direce.Left,
        ConsoleKey.RightArrow when currentDirece != Direce.Left => Direce.Right,
        _ => currentDirece
      };

      return currentDirece;
    }

    static void PrintPerimetr()
    {
      for (int i = 0; i < SizeVertical; i++)
      {
        new Point(i, 0, PerimetrColor).Print();
        new Point(i, SizeHorizontal - 1, PerimetrColor).Print();
      }
      for (int i = 0; i < SizeHorizontal; i++)
      {
        new Point(0, i, PerimetrColor).Print();
        new Point(SizeVertical - 1, i, PerimetrColor).Print();
      }
    }
  }
}
using System;

namespace SnakeGame
{
  public readonly struct Point
  {
    public Point(int x, int y, ConsoleColor color)
    {
      X = x;
      Y = y;
      Color = color;
    }

    public int X { get; }
    public int Y { get; }

    public ConsoleColor Color { get; }

    public void Print()
    {
      Console.ForegroundColor = Color;
      Console.SetCursorPosition(X, Y);
      Console.Write('█');
    }

    public void Clear()
    {
      Console.SetCursorPosition(X, Y);
      Console.Write(' ');
    }
  }
}
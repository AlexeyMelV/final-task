using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
  public class Snake
  {
    private readonly ConsoleColor _headColor;

    private readonly ConsoleColor _bodyColor;

    public Snake(int initialX, int initialY, ConsoleColor headColor, ConsoleColor bodyColor, int bodyLength = 3)
    {
      _headColor = headColor;
      _bodyColor = bodyColor;

      Head = new Point(initialX, initialY, headColor);

      for (int i = bodyLength; i >= 0; i--)
      {
        Body.Enqueue(new Point(Head.X - i - 1, initialY, _bodyColor));
      }

      Print();
    }

    public Point Head { get; private set; }

    public Queue<Point> Body { get; } = new Queue<Point>();

    public void Move(Direce direce, bool eat = false)
    {
      Clear();

      Body.Enqueue(new Point(Head.X, Head.Y, _bodyColor));
      if (!eat)
        Body.Dequeue();

      Head = direce switch
      {
        Direce.Right => new Point(Head.X + 1, Head.Y, _headColor),
        Direce.Left => new Point(Head.X - 1, Head.Y, _headColor),
        Direce.Up => new Point(Head.X, Head.Y - 1, _headColor),
        Direce.Down => new Point(Head.X, Head.Y + 1, _headColor),
        _ => Head
      };

      Print();
    }

    public void Print()
    {
      Head.Print();

      foreach (Point point in Body)
      {
        point.Print();
      }
    }

    public void Clear()
    {
      Head.Clear();

      foreach (Point point in Body)
      {
        point.Clear();
      }
    }
  }
}
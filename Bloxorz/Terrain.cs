// Copyright (c) Timofei Zhakov. All rights reserved.

using Microsoft.Xna.Framework;

namespace Bloxorz
{
    public class Terrain
    {
        public int Width { get; }
        public int Height { get; }
        public Player Player { get; private set; }
        public int EndTimer { get; set; }
        public GameState State { get; set; }

        private readonly Cell[] data;

        public Terrain(Level level)
        {
            Width = level.Width;
            Height = level.Height;
            data = new Cell[Width * Height];
            for (int i = 0; i < Width * Height; i++)
            {
                char c = level.Data[i];
                CellType type = CellType.None;
                if (c == '#')
                {
                    type = CellType.Brick;
                }
                else if (c == 'e')
                {
                    type = CellType.Exit;
                }

                data[i] = new Cell(type);
            }

            EndTimer = -1;

            Player = new Player(this);
        }

        public Cell GetCell(Point point)
        {
            return GetCell(point.X, point.Y);
        }

        public Cell GetCell(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                return new Cell(CellType.None);
            }
            else
            {
                return data[y * Width + x];
            }
        }

        public void Restart()
        {
            Player = new Player(this);
        }

        public void Update()
        {
            Player.Update();

            if (State != GameState.Playing && EndTimer == -1)
            {
                EndTimer = 0;
            }

            if (EndTimer != -1)
            {
                EndTimer++;
            }
        }
    }

    public enum GameState
    {
        Playing,
        GameOver,
        Win
    }

    public class Cell
    {
        public CellType Type;

        public Cell(CellType type)
        {
            Type = type;
        }
    }

    public enum CellType
    {
        None,
        Brick,
        Exit
    }
}

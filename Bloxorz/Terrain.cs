// Copyright (c) Timofei Zhakov. All rights reserved.

using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Bloxorz
{
    public class Terrain
    {
        public int Width { get; }
        public int Height { get; }
        public Player Player { get; }
        public int EndTimer { get; private set; }
        public GameState State { get; set; }

        private readonly Cell[] data;

        public Terrain(Level level)
        {
            Width = level.Width;
            Height = level.Height;
            data = new Cell[Width * Height];
            int currentButton = 0;

            for (int i = 0; i < Width * Height; i++)
            {
                char c = level.Data[i];

                if (c == '.')
                {
                    data[i] = new Cell(CellType.Brick);
                }
                else if (c == 'e')
                {
                    data[i] = new Cell(CellType.Exit);
                }
                else if (c == 's')
                {
                    data[i] = new Cell(CellType.Brick);
                    Player = new Player(this, i % Width, i / Width);
                }
                else if (c == 'b')
                {
                    data[i] = level.Buttons[currentButton];
                    currentButton++;
                }
                else if (c == '@')
                {
                    data[i] = new Cell(CellType.Bridge);
                }
                else
                {
                    data[i] = new Cell(CellType.None);
                }
            }

            if (Player == null)
            {
                throw new Exception("Spawn point not found.");
            }

            EndTimer = -1;
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

        public Cell GetCell(Point point)
        {
            return GetCell(point.X, point.Y);
        }

        public void Update()
        {
            for (int i = 0;  i < Height * Width; i++)
            {
                if (data[i].Animation != -1)
                {
                    data[i].Animation++;
                }
            }

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

    public class PlayerCells
    {
        public Cell Cell1;
        public Cell Cell2;
        public Cell[] Cells;

        public PlayerCells(Cell cell1, Cell cell2 = null)
        {
            Cell1 = cell1;
            Cell2 = cell2;

            if (cell2 == null)
            {
                Cells = [cell1];
            }
            else
            {
                Cells = [cell1, cell2];
            }
        }
    }

    public class Cell
    {
        public CellType Type;
        public bool IsOpen;
        public bool StayRequiered;
        public Point ButtonTarget1;
        public Point ButtonTarget2;
        public int Animation = -1;

        public Cell(CellType type)
        {
            Type = type;
        }
    }

    public enum CellType
    {
        None,
        Brick,
        Exit,
        Button,
        Bridge,
    }
}

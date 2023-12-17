// Copyright (c) Timofei Zhakov. All rights reserved.

using Microsoft.Xna.Framework;

namespace Bloxorz
{
    public class Level
    {
        public int Width;
        public int Height;
        public string Data;

        public CellType GetCell(Point point)
        {
            return GetCell(point.X, point.Y);
        }

        public CellType GetCell(int x, int y)
        {
            if (x < 0 || y < 0 ||
                x > Width || y > Height)
            {
                return CellType.None;
            }
            else
            {
                char c = Data[y * Width + x];
                if (c == '#')
                {
                    return CellType.Brick;
                }
                else
                {
                    return CellType.None;
                }
            }
        }
    }

    public enum CellType
    {
        None,
        Brick
    }
}

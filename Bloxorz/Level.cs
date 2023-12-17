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
            if (point.X < 0 || point.Y < 0 ||
                point.X > Width || point.Y > Height)
            {
                return CellType.None;
            }
            else
            {
                char c = Data[point.Y * Width + point.X];
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

// Copyright (c) Timofei Zhakov. All rights reserved.

namespace Bloxorz
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
    }

    public enum Axis
    {
        Horizontal,
        Vertical,
    }

    public static class DirectionExtensions
    {
        public static int DeltaX(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    return -1;
                case Direction.Right:
                    return 1;
                default:
                    return 0;
            }
        }

        public static int DeltaX(this Direction? direction)
        {
            if (direction.HasValue)
            {
                return direction.Value.DeltaX();
            }
            else
            {
                return 0;
            }
        }

        public static int DeltaY(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    return -1;
                case Direction.Down:
                    return 1;
                default:
                    return 0;
            }
        }

        public static int DeltaY(this Direction? direction)
        {
            if (direction.HasValue)
            {
                return direction.Value.DeltaY();
            }
            else
            {
                return 0;
            }
        }

        public static Direction Reverse(this Direction direction)
        {
            switch (direction)
            {
                case Direction.Left: return Direction.Right;
                case Direction.Right: return Direction.Left;
                case Direction.Up: return Direction.Down;
                case Direction.Down: return Direction.Up;
                default: return direction;
            }
        }

        public static Direction? Reverse(this Direction? direction)
        {
            if (direction.HasValue)
            {
                return direction.Value.Reverse();
            }
            else
            {
                return null;
            }
        }

        public static Axis? GetAxis(this Direction? direction)
        {
            switch (direction)
            {
                case Direction.Left:
                case Direction.Right:
                    return Axis.Horizontal;
                case Direction.Up:
                case Direction.Down:
                    return Axis.Vertical;
                default:
                    return null;
            }
        }
    }
}

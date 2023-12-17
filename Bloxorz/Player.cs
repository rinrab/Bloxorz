// Copyright (c) Timofei Zhakov. All rights reserved.

using Microsoft.Xna.Framework;
using System.Linq;

namespace Bloxorz
{
    public class Player
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Rotation = Vector3.Zero;
        public Direction? Direction;
        public State State = State.Stand;

        private Vector3 delta = Vector3.Zero;
        private int animation = 0;
        private bool isAlive = true;
        private readonly Level level;

        public Player(Level level)
        {
            this.level = level;
        }

        public void Update()
        {
            if (isAlive)
            {
                Move();

                if (animation == -1)
                {
                    CellType[] cells = GetCells();

                    if (cells.Contains(CellType.None))
                    {
                        isAlive = false;
                    }
                }
            }
            else
            {
                Position.Y -= 4;
                Position += delta * 1;
                Rotation.Z -= delta.X.Normalize() * 4;
                Rotation.X += delta.Z.Normalize() * 4;
            }
        }

        private void Move()
        {
            if (animation == -1)
            {
                if (Direction.HasValue)
                {
                    if (State == State.Stand)
                    {
                        delta = new Vector3(Direction.DeltaX() * 1.5f, -0.5f, Direction.DeltaY() * 1.5f);

                        State = (Direction.GetAxis() == Axis.Horizontal) ? State.Horizontal : State.Vertical;
                    }
                    else if ((State == State.Horizontal && Direction.GetAxis() == Axis.Horizontal) ||
                             (State == State.Vertical && Direction.GetAxis() == Axis.Vertical))
                    {
                        delta = new Vector3(Direction.DeltaX() * 1.5f, 0.5f, Direction.DeltaY() * 1.5f);
                        State = State.Stand;
                    }
                    else
                    {
                        delta = new Vector3(Direction.DeltaX() * 1, 0, Direction.DeltaY() * 1);
                    }

                    animation = 0;
                }
            }

            if (animation != -1)
            {
                const float speed = 2;
                if (animation * speed < 16)
                {
                    Position += delta * speed;

                    Rotation.Z = (Rotation.Z - delta.X.Normalize() * speed) % ((State == State.Vertical) ? 16 : 32);
                    Rotation.X = (Rotation.X + delta.Z.Normalize() * speed) % ((State == State.Horizontal) ? 16 : 32);

                    animation++;
                }
                else
                {
                    animation = -1;
                }
            }
        }

        private CellType[] GetCells()
        {
            Point pos = new Point((int)Position.X, (int)Position.Z);

            if (State == State.Horizontal)
            {
                return [
                    level.GetCell((pos.X - 8) / 16, pos.Y / 16),
                    level.GetCell((pos.X - 8) / 16 + 1, pos.Y / 16)
                ];
            }
            else if (State == State.Vertical)
            {
                return [
                    level.GetCell(pos.X / 16, (pos.Y - 8) / 16),
                    level.GetCell(pos.X / 16, (pos.Y - 8) / 16 + 1),
                ];
            }
            else
            {
                return [
                    level.GetCell(pos.X / 16, pos.Y / 16)
                ];
            }
        }
    }

    public enum State
    {
        Stand,
        Vertical,
        Horizontal
    }
}

// Copyright (c) Timofei Zhakov. All rights reserved.

using Microsoft.Xna.Framework;

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
                    Point pos = new Point((int)Position.X, (int)Position.Z);
                    Point cell1 = new Point(pos.X / 16, pos.Y / 16);
                    Point cell2 = new Point(pos.X / 16, pos.Y / 16);

                    if (State == State.Horizontal)
                    {
                        cell1 = new Point((pos.X - 8) / 16, pos.Y / 16);
                        cell2 = new Point((pos.X - 8) / 16 + 1, pos.Y / 16);
                    }
                    if (State == State.Vertical)
                    {
                        cell1 = new Point(pos.X / 16, (pos.Y - 8) / 16);
                        cell2 = new Point(pos.X / 16, (pos.Y - 8) / 16 + 1);
                    }

                    if (level.GetCell(cell1) == CellType.None || level.GetCell(cell2) == CellType.None)
                    {
                        isAlive = false;
                    }
                }
            }
            else
            {
                Position.Y -= 4;
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

                    Rotation.Z = (Rotation.Z - GetSign(delta.X) * speed) % ((State == State.Vertical) ? 16 : 32);
                    Rotation.X = (Rotation.X + GetSign(delta.Z) * speed) % ((State == State.Horizontal) ? 16 : 32);

                    animation++;
                }
                else
                {
                    animation = -1;
                }
            }
        }

        private int GetSign(float number)
        {
            if (number < 0)
            {
                return -1;
            }
            else if (number > 0)
            {
                return 1;
            }
            else
            {
                return 0;
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

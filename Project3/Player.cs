// Copyright (c) Timofei Zhakov. All rights reserved.

using System.Numerics;

namespace Project3
{
    public class Player
    {
        public Vector3 Position = Vector3.Zero;
        public Vector3 Rotation = Vector3.Zero;
        public Direction? Direction;
        public State State = State.Stand;

        private Vector3 delta = Vector3.Zero;
        private int animation = 0;

        public void Update()
        {
            if (animation == -1)
            {
                if (Direction.HasValue)
                {
                    if (State == State.Stand)
                    {
                        delta = new Vector3(Direction.DeltaX() * 3, -1, Direction.DeltaY() * 3);

                        State = (Direction.GetAxis() == Axis.Horizontal) ? State.Horizontal : State.Vertical;
                    }
                    else if ((State == State.Horizontal && Direction.GetAxis() == Axis.Horizontal) ||
                             (State == State.Vertical && Direction.GetAxis() == Axis.Vertical))
                    {
                        delta = new Vector3(Direction.DeltaX() * 3, 1, Direction.DeltaY() * 3);
                        State = State.Stand;
                    }
                    else
                    {
                        delta = new Vector3(Direction.DeltaX() * 2, 0, Direction.DeltaY() * 2);
                    }

                    animation = 0;
                }
            }

            if (animation != -1)
            {
                const float speed = 2;
                if (animation * speed < 16)
                {
                    Position += delta;

                    if (delta.X != 0)
                    {
                        Rotation.Z -= (delta.X > 0) ? speed : -speed;
                        Rotation.Z %= 32;
                    }

                    if (delta.Z != 0)
                    {
                        Rotation.X += (delta.Z > 0) ? speed : -speed;
                        Rotation.X %= 32;
                    }

                    if (State == State.Vertical)
                    {
                        Rotation.Z %= 16;
                    }
                    if (State == State.Horizontal)
                    {
                        Rotation.X %= 16;
                    }

                    animation++;
                }
                else
                {
                    animation = -1;
                }
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

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
        public PlayerState State = PlayerState.Stand;

        private Vector3 delta = Vector3.Zero;
        private int animation = 0;
        private readonly Terrain terrain;
        private readonly float speed = 2;

        public Player(Terrain terrain, int x, int y)
        {
            this.terrain = terrain;
            Position = new Vector3(x * 16, 0, y * 16);
        }

        public void Update()
        {
            if (terrain.State == GameState.Playing)
            {
                Move();

                if (animation == -1)
                {
                    PlayerCells cells = GetCells();

                    if (cells.Cells.Any(cell => cell.Type == CellType.None || (cell.Type == CellType.Bridge && !cell.IsOpen)))
                    {
                        terrain.State = GameState.GameOver;
                    }
                    else if (cells.Cells.Any(cell => cell.Type == CellType.Exit) && State == PlayerState.Stand)
                    {
                        terrain.State = GameState.Win;
                        delta = new Vector3(0, -1, 0);
                    }

                    foreach (Cell cell in cells.Cells)
                    {
                        if (cell.Type == CellType.Button && (!cell.StayRequiered || State == PlayerState.Stand))
                        {
                            terrain.GetCell(cell.ButtonTarget1).IsOpen = true;
                            terrain.GetCell(cell.ButtonTarget2).IsOpen = true;
                        }
                    }
                }
            }
            else
            {
                Position.Y -= 2 * speed;
                Position += delta * speed / 2;
                Rotation.Z -= delta.X.Normalize() * 2 * speed;
                Rotation.X += delta.Z.Normalize() * 2 * speed;
            }
        }

        private void Move()
        {
            if (animation == -1)
            {
                if (Direction.HasValue)
                {
                    if (State == PlayerState.Stand)
                    {
                        delta = new Vector3(Direction.DeltaX() * 1.5f, -0.5f, Direction.DeltaY() * 1.5f);

                        State = (Direction.GetAxis() == Axis.Horizontal) ? PlayerState.Horizontal : PlayerState.Vertical;
                    }
                    else if ((State == PlayerState.Horizontal && Direction.GetAxis() == Axis.Horizontal) ||
                             (State == PlayerState.Vertical && Direction.GetAxis() == Axis.Vertical))
                    {
                        delta = new Vector3(Direction.DeltaX() * 1.5f, 0.5f, Direction.DeltaY() * 1.5f);
                        State = PlayerState.Stand;
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
                if (animation * speed < 16)
                {
                    Position += delta * speed;

                    Rotation.Z = (Rotation.Z - delta.X.Normalize() * speed) % ((State == PlayerState.Vertical) ? 16 : 32);
                    Rotation.X = (Rotation.X + delta.Z.Normalize() * speed) % ((State == PlayerState.Horizontal) ? 16 : 32);

                    animation++;
                }
                else
                {
                    animation = -1;
                }
            }
        }

        private PlayerCells GetCells()
        {
            Point pos = new Point((int)Position.X, (int)Position.Z);

            if (State == PlayerState.Horizontal)
            {
                return new PlayerCells(terrain.GetCell((pos.X - 8) / 16, pos.Y / 16),
                                     terrain.GetCell((pos.X - 8) / 16 + 1, pos.Y / 16));
            }
            else if (State == PlayerState.Vertical)
            {
                return new PlayerCells(terrain.GetCell(pos.X / 16, (pos.Y - 8) / 16),
                                     terrain.GetCell(pos.X / 16, (pos.Y - 8) / 16 + 1));
            }
            else
            {
                return new PlayerCells(terrain.GetCell(pos.X / 16, pos.Y / 16));
            }
        }
    }

    public enum PlayerState
    {
        Stand,
        Vertical,
        Horizontal
    }
}

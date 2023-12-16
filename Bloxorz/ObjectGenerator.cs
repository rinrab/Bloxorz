// Copyright (c) Timofei Zhakov. All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Project3
{
    public static class ObjectGenerator
    {
        public const float BlockSize = 16f;

        public static VertexPositionColorNormal[] GenerateCube(Vector3 pos, Matrix matrix, Vector3 size, Color color)
        {
            VertexPositionColorNormal create(Vector3 vecPos, Vector3 normal)
            {
                normal *= size;
                normal.Normalize();
                return new VertexPositionColorNormal(Vector3.Transform(vecPos * size + pos, matrix), color, normal);
            }

            return [
                create(new Vector3(0, 0, 0), new Vector3(0, -1, 0)),
                create(new Vector3(1, 0, 0), new Vector3(0, -1, 0)),
                create(new Vector3(1, 0, 1), new Vector3(0, -1, 0)),
                create(new Vector3(1, 0, 1), new Vector3(0, -1, 0)),
                create(new Vector3(0, 0, 1), new Vector3(0, -1, 0)),
                create(new Vector3(0, 0, 0), new Vector3(0, -1, 0)),

                create(new Vector3(0, 1, 0), new Vector3(0, 1, 0)),
                create(new Vector3(1, 1, 0), new Vector3(0, 1, 0)),
                create(new Vector3(1, 1, 1), new Vector3(0, 1, 0)),
                create(new Vector3(1, 1, 1), new Vector3(0, 1, 0)),
                create(new Vector3(0, 1, 1), new Vector3(0, 1, 0)),
                create(new Vector3(0, 1, 0), new Vector3(0, 1, 0)),

                create(new Vector3(0, 0, 0), new Vector3(0, 0, -1)),
                create(new Vector3(1, 0, 0), new Vector3(0, 0, -1)),
                create(new Vector3(1, 1, 0), new Vector3(0, 0, -1)),
                create(new Vector3(1, 1, 0), new Vector3(0, 0, -1)),
                create(new Vector3(0, 1, 0), new Vector3(0, 0, -1)),
                create(new Vector3(0, 0, 0), new Vector3(0, 0, -1)),

                create(new Vector3(0, 0, 1), new Vector3(0, 0, 1)),
                create(new Vector3(1, 0, 1), new Vector3(0, 0, 1)),
                create(new Vector3(1, 1, 1), new Vector3(0, 0, 1)),
                create(new Vector3(1, 1, 1), new Vector3(0, 0, 1)),
                create(new Vector3(0, 1, 1), new Vector3(0, 0, 1)),
                create(new Vector3(0, 0, 1), new Vector3(0, 0, 1)),

                create(new Vector3(0, 0, 0), new Vector3(-1, 0, 1)),
                create(new Vector3(0, 1, 0), new Vector3(-1, 0, 1)),
                create(new Vector3(0, 1, 1), new Vector3(-1, 0, 1)),
                create(new Vector3(0, 1, 1), new Vector3(-1, 0, 1)),
                create(new Vector3(0, 0, 1), new Vector3(-1, 0, 1)),
                create(new Vector3(0, 0, 0), new Vector3(-1, 0, 1)),

                create(new Vector3(1, 0, 0), new Vector3(1, 0, 0)),
                create(new Vector3(1, 1, 0), new Vector3(1, 0, 0)),
                create(new Vector3(1, 1, 1), new Vector3(1, 0, 0)),
                create(new Vector3(1, 1, 1), new Vector3(1, 0, 0)),
                create(new Vector3(1, 0, 1), new Vector3(1, 0, 0)),
                create(new Vector3(1, 0, 0), new Vector3(1, 0, 0)),
            ];
        }

        public static VertexBuffer GenerateLevel(GraphicsDevice graphicsDevice, Level level)
        {
            List<VertexPositionColorNormal> vertices = new List<VertexPositionColorNormal>();

            for (int y = 0; y < level.Height; y++)
            {
                for (int x = 0; x < level.Width; x++)
                {
                    int i = y * level.Width + x;

                    if (level.Data[i] == '#')
                    {
                        vertices.AddRange(GenerateCube(new Vector3(x, 0, y) * BlockSize,
                                                       Matrix.CreateTranslation(Vector3.Zero),
                                                       new Vector3(BlockSize, -BlockSize / 4, BlockSize),
                                                       Color.Blue));
                    }
                }
            }

            VertexBuffer vertexBuffer = new VertexBuffer(graphicsDevice,
                                                         typeof(VertexPositionColorNormal),
                                                         vertices.Count,
                                                         BufferUsage.WriteOnly);

            vertexBuffer.SetData(vertices.ToArray());
            return vertexBuffer;
        }

        public static VertexBuffer GeneratePlayer(GraphicsDevice graphicsDevice, Player player)
        {
            var rot = player.Rotation / 16 * MathF.PI / 2;

            Vector3 offset = new Vector3(-8, -16, -8);

            Matrix matrix = Matrix.CreateScale(new Vector3(1, 2, 1) * BlockSize);
            matrix *= Matrix.CreateTranslation(offset);
            if (player.State == State.Vertical)
            {
                matrix *= Matrix.CreateRotationX(rot.X);
                matrix *= Matrix.CreateRotationZ(rot.Z);
            }
            else
            {
                matrix *= Matrix.CreateRotationZ(rot.Z);
                matrix *= Matrix.CreateRotationX(rot.X);
            }
            matrix *= Matrix.CreateTranslation(player.Position - offset);

            var vertices = GenerateCube(Vector3.Zero,
                                        matrix,
                                        Vector3.One,
                                        new Color(0x3b, 0x2d, 0x2f));

            VertexBuffer vertexBuffer = new VertexBuffer(graphicsDevice,
                                                         typeof(VertexPositionColorNormal),
                                                         vertices.Length,
                                                         BufferUsage.WriteOnly);

            vertexBuffer.SetData(vertices);
            return vertexBuffer;
        }
    }
}

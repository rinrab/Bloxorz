﻿// Copyright (c) Timofei Zhakov. All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Bloxorz
{
    public static class ObjectGenerator
    {
        public const float BlockSize = 16f;

        public static VertexPositionColorNormal[] GenerateCube(Vector3 pos, Vector3 size, Vector3 rotation, bool reverseRotation, Color color)
        {
            var matrix =
                Matrix.CreateScale(size) *
                Matrix.CreateTranslation(-size / 2) *
                    (reverseRotation ?
                    Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationZ(rotation.Z) :
                    Matrix.CreateRotationZ(rotation.Z) * Matrix.CreateRotationX(rotation.X)) *
                Matrix.CreateTranslation(size / 2) *
                Matrix.CreateTranslation(pos);

            VertexPositionColorNormal create(Vector3 vecPos, Vector3 normal)
            {
                return new VertexPositionColorNormal(Vector3.Transform(vecPos, matrix), color, Vector3.Zero);
            }

            VertexPositionColorNormal[] rv = [
                create(new Vector3(0, 0, 0), new Vector3(0, -1, 0)),
                create(new Vector3(1, 0, 0), new Vector3(0, -1, 0)),
                create(new Vector3(1, 0, 1), new Vector3(0, -1, 0)),
                create(new Vector3(1, 0, 1), new Vector3(0, -1, 0)),
                create(new Vector3(0, 0, 1), new Vector3(0, -1, 0)),
                create(new Vector3(0, 0, 0), new Vector3(0, -1, 0)),

                create(new Vector3(1, 1, 0), new Vector3(0, 1, 0)),
                create(new Vector3(0, 1, 0), new Vector3(0, 1, 0)),
                create(new Vector3(1, 1, 1), new Vector3(0, 1, 0)),
                create(new Vector3(0, 1, 1), new Vector3(0, 1, 0)),
                create(new Vector3(1, 1, 1), new Vector3(0, 1, 0)),
                create(new Vector3(0, 1, 0), new Vector3(0, 1, 0)),

                create(new Vector3(1, 0, 0), new Vector3(0, 0, -1)),
                create(new Vector3(0, 0, 0), new Vector3(0, 0, -1)),
                create(new Vector3(1, 1, 0), new Vector3(0, 0, -1)),
                create(new Vector3(0, 1, 0), new Vector3(0, 0, -1)),
                create(new Vector3(1, 1, 0), new Vector3(0, 0, -1)),
                create(new Vector3(0, 0, 0), new Vector3(0, 0, -1)),

                create(new Vector3(0, 0, 1), new Vector3(0, 0, 1)),
                create(new Vector3(1, 0, 1), new Vector3(0, 0, 1)),
                create(new Vector3(1, 1, 1), new Vector3(0, 0, 1)),
                create(new Vector3(1, 1, 1), new Vector3(0, 0, 1)),
                create(new Vector3(0, 1, 1), new Vector3(0, 0, 1)),
                create(new Vector3(0, 0, 1), new Vector3(0, 0, 1)),

                create(new Vector3(0, 1, 0), new Vector3(-1, 0, 1)),
                create(new Vector3(0, 0, 0), new Vector3(-1, 0, 1)),
                create(new Vector3(0, 1, 1), new Vector3(-1, 0, 1)),
                create(new Vector3(0, 0, 1), new Vector3(-1, 0, 1)),
                create(new Vector3(0, 1, 1), new Vector3(-1, 0, 1)),
                create(new Vector3(0, 0, 0), new Vector3(-1, 0, 1)),

                create(new Vector3(1, 0, 0), new Vector3(1, 0, 0)),
                create(new Vector3(1, 1, 0), new Vector3(1, 0, 0)),
                create(new Vector3(1, 1, 1), new Vector3(1, 0, 0)),
                create(new Vector3(1, 1, 1), new Vector3(1, 0, 0)),
                create(new Vector3(1, 0, 1), new Vector3(1, 0, 0)),
                create(new Vector3(1, 0, 0), new Vector3(1, 0, 0)),
            ];

            for (int i = 0; i < rv.Length; i += 3)
            {
                Vector3 dir = Vector3.Cross(rv[i + 1].Position - rv[i + 0].Position, rv[i + 2].Position - rv[i + 0].Position);
                Vector3 norm = Vector3.Normalize(dir);
                rv[i + 0].Normal = norm;
                rv[i + 1].Normal = norm;
                rv[i + 2].Normal = norm;
            }

            return rv;
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
                        vertices.AddRange(GenerateCube(new Vector3(x * BlockSize, -BlockSize / 4, y * BlockSize),
                                                       new Vector3(BlockSize, BlockSize / 4, BlockSize),
                                                       Vector3.Zero, false, Color.Blue));
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
            var vertices = GenerateCube(player.Position,
                                        new Vector3(1, 2, 1) * BlockSize,
                                        player.Rotation / 16 * MathF.PI / 2,
                                        player.State == State.Vertical,
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

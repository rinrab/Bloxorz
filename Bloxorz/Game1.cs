// Copyright (c) Timofei Zhakov. All rights reserved.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Bloxorz
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private Vector3 camPosition;
        private Vector3 camRotation;
        private Matrix projectionMatrix;
        private Matrix worldMatrix;
        private Effect effect;

        private Terrain terrain;
        private int level = 3;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height,
                //IsFullScreen = true,
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            projectionMatrix = Matrix.CreateOrthographic(16 * 1 * 16, 16 * 1 * 9, 1, 4096);

            worldMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up);

            effect = Content.Load<Effect>("shader");
            effect.Parameters["World"].SetValue(worldMatrix);
            effect.Parameters["Projection"].SetValue(projectionMatrix);
            effect.Parameters["Light"].SetValue(new Vector3(-2, 3, 1));
            effect.Parameters["Texture"].SetValue(Content.Load<Texture2D>("Texture"));

            graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            NewGame();

            base.Initialize();
        }

        private void NewGame()
        {
            terrain = new Terrain(LevelData.Levels[level]);
        }

        protected override void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Left))
                terrain.Player.Direction = Direction.Left;
            else if (state.IsKeyDown(Keys.Right))
                terrain.Player.Direction = Direction.Right;
            else if (state.IsKeyDown(Keys.Up))
                terrain.Player.Direction = Direction.Up;
            else if (state.IsKeyDown(Keys.Down))
                terrain.Player.Direction = Direction.Down;
            else if (state.IsKeyDown(Keys.R))
                NewGame();
            else if (state.IsKeyDown(Keys.Escape))
                Exit();
            else
                terrain.Player.Direction = null;

            terrain.Update();

            if (terrain.EndTimer > 30)
            {
                if (terrain.State == GameState.GameOver)
                {
                    NewGame();
                }
                else if (terrain.State == GameState.Win)
                {
                    level++;
                    NewGame();
                }
            }

            camRotation = new Vector3(20, 10, 0) * (float)Math.PI / 180f;
            camPosition = new Vector3(0.1f, -0.3f, 1) * terrain.Width * 30;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            effect.Parameters["View"].SetValue(
                Matrix.CreateTranslation(new Vector3(-camPosition.X, camPosition.Y, -camPosition.Z)) *
                Matrix.CreateRotationY(camRotation.Y) *
                Matrix.CreateRotationX(camRotation.X));

            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.RasterizerState = new RasterizerState
            {
                CullMode = CullMode.CullClockwiseFace
            };

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                using (VertexBuffer levelVertexBuffer = ObjectGenerator.GenerateLevel(GraphicsDevice, terrain))
                {
                    GraphicsDevice.SetVertexBuffer(levelVertexBuffer);
                    GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, levelVertexBuffer.VertexCount);
                }

                using (VertexBuffer playerVertexBuffer = ObjectGenerator.GeneratePlayer(GraphicsDevice, terrain.Player))
                {
                    GraphicsDevice.SetVertexBuffer(playerVertexBuffer);
                    GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, playerVertexBuffer.VertexCount);
                }
            }

            base.Draw(gameTime);
        }
    }
}

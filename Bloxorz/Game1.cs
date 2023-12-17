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

        private VertexBuffer levelVertexBuffer;

        const float BlockSize = 16;

        private Player player;

        private Level currentLevel;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1280,
                PreferredBackBufferHeight = 720
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            currentLevel = LevelData.Levels[0];
            player = new Player(currentLevel);
        }

        protected override void Initialize()
        {
            projectionMatrix = Matrix.CreateOrthographic(BlockSize * 1 * 16, BlockSize * 1 * 9, 1, 4096);

            worldMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up);

            effect = Content.Load<Effect>("shader");

            graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            levelVertexBuffer = ObjectGenerator.GenerateLevel(GraphicsDevice, currentLevel);

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Left))
                player.Direction = Direction.Left;
            else if (state.IsKeyDown(Keys.Right))
                player.Direction = Direction.Right;
            else if (state.IsKeyDown(Keys.Up))
                player.Direction = Direction.Up;
            else if (state.IsKeyDown(Keys.Down))
                player.Direction = Direction.Down;
            else if (state.IsKeyDown(Keys.R))
                player = new Player(currentLevel);
            else
                player.Direction = null;

            player.Update();

            camRotation = new Vector3(20, 10, 0) * (float)Math.PI / 180f;
            camPosition = new Vector3(0.1f, -0.3f, 1) * 300;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            effect.Parameters["Projection"].SetValue(projectionMatrix);
            effect.Parameters["View"].SetValue(
                Matrix.CreateTranslation(new Vector3(-camPosition.X, camPosition.Y, -camPosition.Z)) *
                Matrix.CreateRotationY(camRotation.Y) *
                Matrix.CreateRotationX(camRotation.X));
            effect.Parameters["World"].SetValue(worldMatrix);

            GraphicsDevice.Clear(Color.CornflowerBlue);

            GraphicsDevice.RasterizerState = new RasterizerState
            {
                CullMode = CullMode.CullClockwiseFace
            };

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.SetVertexBuffer(levelVertexBuffer);
                GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, levelVertexBuffer.VertexCount);

                using (VertexBuffer playerVertexBuffer = ObjectGenerator.GeneratePlayer(GraphicsDevice, player))
                {
                    GraphicsDevice.SetVertexBuffer(playerVertexBuffer);
                    GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, playerVertexBuffer.VertexCount);
                }
            }

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            levelVertexBuffer.Dispose();
            base.Dispose(disposing);
        }
    }
}

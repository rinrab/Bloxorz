﻿namespace Project3
{
    internal class Program
    {
        private static void Main()
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
}
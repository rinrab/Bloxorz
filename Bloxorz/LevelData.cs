// Copyright (c) Timofei Zhakov. All rights reserved.

using Microsoft.Xna.Framework;

namespace Bloxorz
{
    public static class LevelData
    {
        public static readonly Level[] Levels = [
            new Level()
            {
                Width = 10,
                Height = 6,
                Data =
                    "###-------" +
                    "#s####----" +
                    "#########-" +
                    "-#########" +
                    "-----##e##" +
                    "------###-"
            },
            new Level()
            {
                Width = 15,
                Height = 6,
                Data =
                    "------####--###" +
                    "####--##b#--#e#" +
                    "##b#--####--###" +
                    "####--####--###" +
                    "#s##@@####@@###" +
                    "####--####-----",
                Buttons = [
                    new Cell(CellType.Button)
                    {
                        StayRequiered = true,
                        ButtonTarget1 = new Point(10, 4),
                        ButtonTarget2 = new Point(11, 4),
                    },
                    new Cell(CellType.Button)
                    {
                        StayRequiered = false,
                        ButtonTarget1 = new Point(4, 4),
                        ButtonTarget2 = new Point(5, 4),
                    },
                ]
            },
            new Level()
            {
                Width = 15,
                Height = 6,
                Data =
                    "------#######--" +
                    "####--###--##--" +
                    "#########--####" +
                    "#s##-------##e#" +
                    "####-------####" +
                    "------------###"
            },
            new Level()
            {
                Width = 10,
                Height = 6,
                Data =
                    "##########" +
                    "#s########" +
                    "##########" +
                    "##########" +
                    "##########" +
                    "##########"
            }
        ];
    }
}

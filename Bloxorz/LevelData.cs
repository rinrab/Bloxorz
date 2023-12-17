// Copyright (c) Timofei Zhakov. All rights reserved.

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
                    "######----" +
                    "#########-" +
                    "-#########" +
                    "-----##e##" +
                    "------###-"
            },
            new Level()
            {
                Width = 10,
                Height = 6,
                Data =
                    "##########" +
                    "##########" +
                    "##########" +
                    "##########" +
                    "##########" +
                    "##########"
            }
        ];
    }
}

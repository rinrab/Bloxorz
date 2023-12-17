// Copyright (c) Timofei Zhakov. All rights reserved.

namespace Bloxorz
{
    public static class NumberExtensions
    {
        public static int Normalize(this float number)
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
}

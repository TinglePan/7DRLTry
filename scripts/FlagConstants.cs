using System;

namespace Proj7DRL.scripts;

public static class FlagConstants
{
    [Flags]
    public enum Direction
    {
        Neutral = 0,
        Up = 1,
        North = Up,
        Right = 2,
        East = Right,
        Down = 4,
        South = Down,
        Left = 8,
        West = Left,
        UpLeft = 9,
        NorthWest = UpLeft,
        UpRight = 3,
        NorthEast = UpRight,
        DownLeft = 12,
        SouthWest = DownLeft,
        DownRight = 6,
        SouthEast = DownRight
    }
}
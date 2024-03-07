using Godot;

namespace Proj7DRL.scripts;

public static class Utils
{
    public static Vector2 MapToWorld(Vector2I mapPos)
    {
        return mapPos * Configuration.TileSize + Configuration.TileSize / 2;
    }
    
    public static Vector2I WorldToMap(Vector2 worldPos)
    {
        return new Vector2I((int)(worldPos.X / Configuration.TileSize.X), (int)(worldPos.Y / Configuration.TileSize.Y));
    }

    public static int Dir2Angle(FlagConstants.Direction dir)
    {
        switch (dir)
        {
            case FlagConstants.Direction.Up:
                return 0;
            case FlagConstants.Direction.Right:
                return 90;
            case FlagConstants.Direction.Down:
                return 180;
            case FlagConstants.Direction.Left:
                return 270;
            case FlagConstants.Direction.UpRight:
                return 45;
            case FlagConstants.Direction.DownRight:
                return 135;
            case FlagConstants.Direction.DownLeft:
                return 225;
            case FlagConstants.Direction.UpLeft:
                return 315;
            default:
                return 0;
        }
    }
    
    public static FlagConstants.Direction RotateDirByDir(FlagConstants.Direction dir, FlagConstants.Direction rotateDir)
    {
        var rotateDirAngle = Dir2Angle(rotateDir);
        while (rotateDirAngle > 0)
        {
            if (rotateDirAngle == 45)
            {
                dir |= RotateDirByClockDir(dir, ClockDirection.Clockwise);
            }
            else
            {
                dir = RotateDirByClockDir(dir, ClockDirection.Clockwise);
            }
            rotateDirAngle -= 90;
        }

        return dir;
    }
    
    public static FlagConstants.Direction RotateDirByClockDir(FlagConstants.Direction dir, ClockDirection clockDir)
    {
        var dirValue = (int)dir;
        int resultDirValue;
        if (clockDir == ClockDirection.Clockwise)
        {
            // Note: The formula is ((value << shiftAmount) | (value >> (maxBits - shiftAmount))) & ((1 << maxBits) - 1), according to GPT. 
            resultDirValue = ((dirValue << 1) | (dirValue >> (4 - 1))) & ((1 << 4) - 1);
        }
        else
        {
            resultDirValue = ((dirValue >> 1) | (dirValue << (4 - 1))) & ((1 << 4) - 1);
        }
        return (FlagConstants.Direction)resultDirValue;
    }
}
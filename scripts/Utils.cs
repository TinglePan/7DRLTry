using System;
using System.Collections.Generic;
using System.Text;
using Godot;
using SadRogue.Primitives;

namespace Proj7DRL.scripts;

public static class Utils
{
    public static Vector2 MapToWorld(Vector2I mapPos)
    {
        return mapPos * Configuration.TileSize + Configuration.TileSize / 2;
    }
    
    public static Vector2I WorldToMap(Vector2 worldPos)
    {
        return new Vector2I(Mathf.FloorToInt(worldPos.X / Configuration.TileSize.X), Mathf.FloorToInt(worldPos.Y / Configuration.TileSize.Y));
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
    
    public static IEnumerable<Vector2I> GetLine(Vector2I start, Vector2I end)
    {
        foreach (var point in Lines.GetBresenhamLine(start.X, start.Y, end.X, end.Y))
        {
            yield return new Vector2I(point.X, point.Y);
        }
    }

    public static bool IsInSameLine(Vector2I a, Vector2I b)
    {
        return a.X == b.X || a.Y == b.Y;
    }
    
    public static string ToSnakeCase(this string text)
    {
        if(text == null) {
            throw new ArgumentNullException(nameof(text));
        }
        if(text.Length < 2) {
            return text.ToLowerInvariant();
        }
        var sb = new StringBuilder();
        sb.Append(char.ToLowerInvariant(text[0]));
        for(int i = 1; i < text.Length; ++i) {
            char c = text[i];
            if(char.IsUpper(c)) {
                sb.Append('_');
                sb.Append(char.ToLowerInvariant(c));
            } else {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    public static Vector2 Forward(Node2D node)
    {
        return node.GlobalTransform.BasisXform(Vector2.Up);
    }
}
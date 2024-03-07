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
    
    public static Vector2I RotateDirVectorByDirVector(Vector2I dirVector, Vector2I rotateVector)
    {
        double angle = Mathf.Atan2(rotateVector.Y, rotateVector.X) - Mathf.Atan2(dirVector.Y, dirVector.X);

        // Ensure the angle is within the range [-π, π]
        if (angle < -Mathf.Pi)
            angle += 2 * Mathf.Pi;
        else if (angle > Mathf.Pi)
            angle -= 2 * Mathf.Pi;
        double x = dirVector.X * Mathf.Cos(angle) - dirVector.Y * Mathf.Sin(angle);
        double y = dirVector.X * Mathf.Sin(angle) + dirVector.Y * Mathf.Cos(angle);
        return new Vector2I(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
    }
}
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
}
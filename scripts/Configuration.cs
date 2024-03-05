using Godot;

namespace Proj7DRL.scripts;

public static class Configuration
{
    public static readonly Vector2I TileSize = new Vector2I(24, 24);
    public const bool StartAtTopLeft = true;
    public const int MapSize = 15;
    public static readonly Vector2I PlayerStartPos = new Vector2I(7, 7);
}
using Godot;

namespace Proj7DRL.scripts;

public partial class Hostile: Node, IProvideDirectionalSprite
{
    private Texture2D _placeholderTexture;
    
    public override void _Ready()
    {
        _placeholderTexture = GD.Load<Texture2D>("res://images/player.png");
    }
    
    public Texture2D GetTextureAtDir(FlagConstants.Direction dir)
    {
        if (dir == FlagConstants.Direction.Neutral)
        {
            return _placeholderTexture;
        }
        else
        {
            return null;
        }
    }
}
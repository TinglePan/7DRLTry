using Godot;

namespace Proj7DRL.scripts;

public interface IProvideDirectionalSprite
{
    public Texture2D GetTextureAtDir(FlagConstants.Direction dir);
}
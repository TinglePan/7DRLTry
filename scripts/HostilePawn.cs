using Godot;
using Proj7DRL.scripts.data_binding;

namespace Proj7DRL.scripts;

public partial class HostilePawn: Node
{
    private Pawn _parent;
    
    public int Tier;
    
    public override void _Ready()
    {
        _parent = GetParent<Pawn>();
    }

    public override void _Process(double delta)
    {
        if (Tier > 0)
        {
            _parent.Sprite.Texture = GD.Load<Texture2D>($"res://images/hostile_t{Tier}.png");
        }
    }
}
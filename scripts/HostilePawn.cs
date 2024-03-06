using Godot;
using Proj7DRL.scripts.data_binding;

namespace Proj7DRL.scripts;

public partial class HostilePawn: Pawn
{
    public ObservableProperty<int> Tier;
    
    public override void _Ready()
    {
        base._Ready();
        Tier = new ObservableProperty<int>("Tier", 0);
        Tier.DetailedValueChanged += (sender, args) =>
        {
            Sprite.Texture = GD.Load<Texture2D>($"res://images/hostile_t{args.NewValue}.png");
        };
    }

    public override void _Process(double delta)
    {
    }
}
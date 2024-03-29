using System;
using Godot;

namespace Proj7DRL.scripts;

public partial class AbilityIcon : TextureRect
{
    private Label _inspectorLabel;
    private Ability _ability;

    public void Setup(Ability ability, Label inspectorLabel)
    {
        _inspectorLabel = inspectorLabel;
        _ability = ability;
        Texture = GD.Load<Texture2D>(ability.IconPath);
    }
	
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        MouseEntered += () =>
        {
            _inspectorLabel.Text = _ability.Description;
        };
        MouseExited += () =>
        {
            _inspectorLabel.Text = "";
        };
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
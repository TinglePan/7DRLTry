using Godot;

namespace Proj7DRL.scripts;

public partial class Tile : Node2D
{
	[Export] public Sprite2D Sprite;
	[Export] public Area2D Collider;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Collider.ProcessMode = ProcessModeEnum.Disabled;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public bool IsOccupied()
	{
		return false;
	}

	public void OnAreaEntered(Area2D area)
	{
		GD.Print("OnAreaEntered Tile");
		area.EmitSignal("area_entered", Collider);
	}
}
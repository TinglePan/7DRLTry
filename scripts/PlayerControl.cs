using Godot;

namespace Proj7DRL.scripts;

public partial class PlayerControl : Node
{
	private Pawn _parent;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_parent = GetParent<Pawn>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	// public override void _Input(InputEvent @event)
	// {
	// 	base._Input(@event);
	// }

	// public void Move(Vector2 direction)
	// {
	// 	var target = GetParent<Map>().Tiles[GlobalPosition.x + (int)direction.x, GlobalPosition.y + (int)direction.y];
	// 	if (target != null)
	// 	{
	// 		GlobalPosition += direction;
	// 	}
	// }
}
using Godot;

namespace Proj7DRL.scripts;

public partial class DbgRaycastDraw : Node2D
{
	
	public Vector2 Start;
	public Vector2 End;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Start = Vector2.Zero;
		End = Vector2.Zero;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Draw()
	{
		if (Start == End) return;
		DrawLine(Start, End, Colors.Red);
	}
}
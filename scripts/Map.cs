using Godot;

namespace Proj7DRL.scripts;

public partial class Map : Node
{
	public Tile[,] Tiles = new Tile[Configuration.MapSize, Configuration.MapSize];
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public bool IsPosInBound(Vector2I pos)
	{
		return pos.X is >= 0 and < Configuration.MapSize && pos.Y is >= 0 and < Configuration.MapSize;
	}
}
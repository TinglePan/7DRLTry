using Godot;

namespace Proj7DRL.scripts;

public partial class GameMgr : Node
{
	[Export] private PackedScene _mapPrefab;

	public Map Map;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Start()
	{
		Map = _mapPrefab.Instantiate() as Map;
		AddChild(Map);
	}
}
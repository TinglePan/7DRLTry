using Godot;

namespace Proj7DRL.scripts;

public partial class GameMgr : Node
{
	[Export] private PackedScene _mapPrefab;
	[Export] private PackedScene _playerPrefab;
	[Export] private PackedScene _enemyPrefab;
	[Export] private PackedScene _controlPanelPrefab;

	public bool Started;
	public Map Map;
	public Pawn PlayerPawn;
	public ControlPanel ControlPanel;
	public int TurnCount { get; private set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Started = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!Started)
		{
			Started = true;
			Start();
		}
	}

	public void Start()
	{
		TurnCount = 0;
		ControlPanel = _controlPanelPrefab.Instantiate() as ControlPanel;
		AddChild(ControlPanel);
		Map = _mapPrefab.Instantiate() as Map;
		AddChild(Map);
		PlayerPawn = _playerPrefab.Instantiate() as Pawn;
		Map?.AddChild(PlayerPawn);
	}

	public void PlayerTurnEnd()
	{
		TurnCount++;
	}
	
	
}
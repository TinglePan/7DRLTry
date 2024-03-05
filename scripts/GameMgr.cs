using System;
using Godot;

namespace Proj7DRL.scripts;

public partial class GameMgr : Node
{
	[Export] private PackedScene _playerPrefab;
	[Export] private PackedScene _enemyPrefab;

	public bool Started;
	public Map Map;
	public Pawn PlayerPawn;
	public ControlPanel ControlPanel;
	public Backpack Backpack;
	public int TurnCount { get; private set; }
	public Random Rand;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Started = false;
		Rand = new Random(DateTime.Now.Millisecond);
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
		ControlPanel = GetNode<ControlPanel>("/root/Main/ControlPanelWindow/ControlPanel");
		Backpack = GetNode<Backpack>("/root/Main/BackpackWindow/Backpack");
		Map = GetNode<Map>("/root/Main/GameWindow/VBoxContainer/MapWrapper/Map");
		PlayerPawn = _playerPrefab.Instantiate() as Pawn;
		Map?.Spawn(PlayerPawn, Configuration.PlayerStartPos);
		Backpack.AddItem(0);
	}

	public void PlayerTurnEnd()
	{
		TurnCount++;
		SpawnEnemyCheck();
	}

	public void SpawnEnemyAtMapEdge()
	{
		var enemyPawn = _enemyPrefab.Instantiate() as Pawn;
		var hostile = enemyPawn?.GetNode<HostilePawn>("Hostile");
		if (hostile != null) hostile.Tier = 1;
		Map.Spawn(enemyPawn, Map.RandomUnoccupiedPosOnBorders());
	}
	
	public int RandomInt(int max, int min=0)
	{
		return Rand.Next(min, max);
	}

	private void SpawnEnemyCheck()
	{
		var checkDone = false;
		while (!checkDone)
		{
			var roll = RandomInt(100);
			if (roll > 80)
			{
				SpawnEnemyAtMapEdge();
			}
			else
			{
				checkDone = true;
			}
		}
	}
	
}
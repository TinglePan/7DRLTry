using System;
using Godot;

namespace Proj7DRL.scripts;

public partial class GameMgr : Node
{
	[Export] private PackedScene _playerPrefab;
	[Export] private PackedScene _enemyPrefab;
	[Export] private PackedScene _bulletPrefab;
	[Export] private PackedScene _laserPrefab;

	public Map Map;
	public PlayerPawn PlayerPawn;
	public AbilityPanel AbilityPanel;
	
	public bool Started;
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
		AbilityPanel = GetNode<AbilityPanel>("/root/Main/AbilityPanelWindow/AbilityPanel");
		Map = GetNode<Map>("/root/Main/GameWindow/VBoxContainer/MapWrapper/Map");
		PlayerPawn = _playerPrefab.Instantiate() as PlayerPawn;
		Map?.SpawnPawn(PlayerPawn, Configuration.PlayerStartPos);
		
		AbilityPanel.AddAbility(0);
	}

	public void PlayerTurnEnd()
	{
		TurnCount++;
		SpawnEnemyCheck();
	}

	public void SpawnEnemyAtMapEdge()
	{
		var enemyPawn = _enemyPrefab.Instantiate() as HostilePawn;
		Map.SpawnPawn(enemyPawn, Map.RandomUnoccupiedPosOnBorders());
		if (enemyPawn != null) enemyPawn.Tier.Value = 1;
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
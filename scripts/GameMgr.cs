using System;
using System.Collections.Generic;
using Godot;
using Proj7DRL.scripts.data_binding;

namespace Proj7DRL.scripts;

public partial class GameMgr : Node
{
	[Export] private PackedScene _playerPrefab;
	[Export] private PackedScene _enemyMeteorPrefab;
	[Export] private PackedScene _enemyTurretPrefab;
	[Export] private PackedScene _enemySniperPrefab;
	[Export] public PackedScene BulletPrefab;
	[Export] public PackedScene MissilePrefab;
	[Export] public PackedScene LaserPrefab;
	
	public GameInfo GameInfo;
	
	public Map Map;
	public PlayerPawn PlayerPawn;
	public AbilityPanel AbilityPanel;
	public Shop Shop;
	public Label InspectorLabel;
	
	public bool Started;
	public int TurnCount { get; private set; }
	public ObservableProperty<int> ScrapCount;
	public Random Rand;

	public HashSet<BaseProjectile> WaitingProjectiles;
	public HashSet<BaseProjectile> DoTProjectiles;
	
	private Queue<Pawn> _destroyedPawns;
	private Queue<BaseProjectile> _destroyedProjectiles;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Started = false;
		ScrapCount = new ObservableProperty<int>("ScrapCount", 0);
		Rand = new Random(DateTime.Now.Millisecond);
		_destroyedPawns = new Queue<Pawn>();
		_destroyedProjectiles = new Queue<BaseProjectile>();
		WaitingProjectiles = new HashSet<BaseProjectile>();
		DoTProjectiles = new HashSet<BaseProjectile>();
		AbilityPanel = GetNode<AbilityPanel>("/root/Main/AbilityPanelWindow/AbilityPanel");
		Map = GetNode<Map>("/root/Main/GameWindow/MapWrapper/Map");
		GameInfo = GetNode<GameInfo>("/root/Main/GameInfoWindow");
		Shop = GetNode<Shop>("/root/Main/GameWindow/Shop");
		InspectorLabel = GetNode<Label>("/root/Main/InspectorWindow/MarginContainer2/MarginContainer/Label");

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
		ScrapCount.Value = 0;
		PlayerPawn = _playerPrefab.Instantiate() as PlayerPawn;
		Map.Clear();
		Map.SpawnPawn(PlayerPawn, Configuration.PlayerStartPos);
		PlayerPawn?.Setup(null);
		AbilityPanel.Clear();
		// AbilityPanel.AddAbility(0);
		AbilityPanel.AddAbility(1);
		GameInfo.Setup(this);
		Shop.Setup(new List<Ability>() { new Ability(0, this) }, AbilityPanel, InspectorLabel, 1);
		SpawnEnemyAtMapEdge();
		
		var dbgButton = GetNode<Button>("/root/Main/GameInfoWindow/DbgButton");
		dbgButton.Pressed += SpawnEnemyAtMapEdge;
	}

	public void PlayerTurnEnd()
	{
		EnemiesTurn();
		TurnEnd();
		TurnCount++;
	}

	public void SpawnEnemyAtMapEdge()
	{
		var rand = Rand.Next(100);
		HostilePawn enemyPawn;
		EnemyDef def;
		var spawnPos = Map.RandomPosOnBorders(true);
		Dictionary<string, Variant> aiSetupArgs;
		// rand = 10;
		switch (rand)
		{
			case < 50:
				enemyPawn = _enemyMeteorPrefab.Instantiate<HostilePawn>();
				def = new EnemyDef { AiName = "MeteorAi", Name = "Space junk", Hp = 2 };
				aiSetupArgs = new ()
				{
					{ "pawn", enemyPawn },
					// { "startPos", spawnPos },
					{ "targetPos",  Map.RandomPosOnBorders()}
				};
				break;
			case >= 50 and < 80:
				enemyPawn = _enemyTurretPrefab.Instantiate<HostilePawn>();
				def = new EnemyDef { AiName = "PotShooterAi", Name = "PotShooter", Hp = 5};
				aiSetupArgs = new ()
				{
					{ "pawn", enemyPawn },
					// { "startPos", spawnPos },
					{ "targetPos",  Map.RandomPosOnBorders() }
				};
				break;
			case >= 80:
				enemyPawn = _enemySniperPrefab.Instantiate<HostilePawn>();
				def = new EnemyDef { AiName = "PreciseShooterAi", Name = "Sniper", Hp = 3 };
				aiSetupArgs = new ()
				{
					{ "pawn", enemyPawn },
					// { "startPos", spawnPos },
					{ "targetPos", Map.RandomPosOnBorders() }
				};
				break;
		}
		Map.SpawnPawn(enemyPawn, spawnPos);
		enemyPawn.Setup(def);
		enemyPawn.Ai.Setup(aiSetupArgs);
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

	private void EnemiesTurn()
	{
		foreach (var pawn in Map.Pawns)
		{
			if (pawn is HostilePawn hostilePawn)
			{
				hostilePawn.Ai?.Proceed();
			}
		}

		// SpawnEnemyCheck();
	}

	private void TurnEnd()
	{
		// SpawnEnemyCheck();
		foreach (var destroyedPawn in _destroyedPawns)
		{
			Map.Pawns.Remove(destroyedPawn);
			destroyedPawn.QueueFree();
		}
		_destroyedPawns.Clear();
		foreach (var projectile in _destroyedProjectiles)
		{
			WaitingProjectiles.Remove(projectile);
			DoTProjectiles.Remove(projectile);
			projectile.QueueFree();
		}
		_destroyedProjectiles.Clear();
	}
	
	public void DestroyPawn(Pawn pawn)
	{
		_destroyedPawns.Enqueue(pawn);
	}
	
	public void DestroyProjectile(BaseProjectile projectile)
	{
		projectile.Visible = false;
		_destroyedProjectiles.Enqueue(projectile);
	}
	
}
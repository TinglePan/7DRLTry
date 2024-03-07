using System;
using System.Collections.Generic;
using Godot;
using Godot.NativeInterop;

namespace Proj7DRL.scripts;

public partial class Map : Node
{
	[Export] private PackedScene _tilePrefab;
	[Export] private Area2D _collider;
	[Export] private CollisionShape2D _colliderShape;
	
	public Tile[,] Tiles;

	private GameMgr _gameMgr;
	private Control _parent;
	private List<Vector2I> _posOnBorders;
	private bool _hasInitialized;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Texture2D groundTexture = GD.Load<Texture2D>("res://images/tile_ground.png");
		_gameMgr = GetNode<GameMgr>("/root/GameMgr");
		_parent = GetParent<Control>();
		_parent.Size = new Vector2(Configuration.MapSize * Configuration.TileSize.X, Configuration.MapSize * Configuration.TileSize.Y);
		_hasInitialized = false;
		Tiles = new Tile[Configuration.MapSize, Configuration.MapSize];
		_posOnBorders = new List<Vector2I>();
		for (int i = 0; i < Configuration.MapSize; i++)
		{
			for (int j = 0; j < Configuration.MapSize; j++)
			{
				var tile = _tilePrefab.Instantiate() as Tile;
				if (tile != null)
				{
					tile.Sprite.Texture = groundTexture;
					tile.Sprite.SelfModulate = (i + j) % 2 == 0 ? new Color("c0c0c0") : new Color("404040");
					tile.Position = Utils.MapToWorld(new Vector2I(i, j));
				}
				Tiles[i, j] = tile;
				AddChild(tile);
				if (i == 0 || i == Configuration.MapSize - 1 || j == 0 || j == Configuration.MapSize - 1)
				{
					if (tile != null) tile.Collider.ProcessMode = ProcessModeEnum.Always;
					_posOnBorders.Add(new Vector2I(i, j));
				}
			}
		}
		((RectangleShape2D)_colliderShape.Shape).Size = new Vector2(Configuration.MapSize * Configuration.TileSize.X, 
			Configuration.MapSize * Configuration.TileSize.Y);
		_colliderShape.Position = new Vector2((float)Configuration.MapSize * Configuration.TileSize.Y / 2, 
			(float)Configuration.MapSize * Configuration.TileSize.Y / 2);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public bool IsPosInBound(Vector2I pos)
	{
		return pos.X is >= 0 and < Configuration.MapSize && pos.Y is >= 0 and < Configuration.MapSize;
	}

	public void SpawnPawn(Pawn pawn, Vector2I pos)
	{
		AddChild(pawn);
		pawn.SetPos(pos);
	}
	
	public Vector2I RandomUnoccupiedPosOnBorders()
	{
		Span<Vector2I> availablePos = stackalloc Vector2I[_posOnBorders.Count];
		var count = 0;
		foreach (var pos in _posOnBorders)
		{
			if (!Tiles[pos.X, pos.Y].IsOccupied())
			{
				availablePos[count++] = pos;
			}
		}
		return availablePos[_gameMgr.RandomInt(count)];
	}

	public void OnAreaExited(Area2D area)
	{
		GD.Print("OnAreaExited Map");
		area.EmitSignal("area_exited", _collider);
	}
}
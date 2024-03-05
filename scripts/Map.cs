using System;
using System.Collections.Generic;
using Godot;
using Godot.NativeInterop;

namespace Proj7DRL.scripts;

public partial class Map : Node
{
	[Export] private PackedScene _tilePrefab; 
	
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
					tile.Position = Utils.MapToWorld(new Vector2I(i, j));
				}
				Tiles[i, j] = tile;
				AddChild(tile);
				if (i == 0 || i == Configuration.MapSize - 1 || j == 0 || j == Configuration.MapSize - 1)
				{
					_posOnBorders.Add(new Vector2I(i, j));
				}
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public bool IsPosInBound(Vector2I pos)
	{
		return pos.X is >= 0 and < Configuration.MapSize && pos.Y is >= 0 and < Configuration.MapSize;
	}

	public void Spawn(Pawn pawn, Vector2I pos)
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
}
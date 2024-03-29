using System;
using System.Collections.Generic;
using Godot;
using Godot.NativeInterop;

namespace Proj7DRL.scripts;

public partial class Map : Node2D
{
	[Export] private PackedScene _tilePrefab;
	[Export] private Area2D _borderCollider;
	// [Export] private CollisionShape2D _colliderShape;
	
	public Tile[,] Tiles;
	public List<Pawn> Pawns;

	private GameMgr _gameMgr;
	private Control _parent;
	private List<Vector2I> _posOnBorders;
	private bool _hasInitialized;
	private Rid _rangeQueryCircleRid;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Texture2D groundTexture = GD.Load<Texture2D>("res://images/tile_ground.png");
		_gameMgr = GetNode<GameMgr>("/root/GameMgr");
		Pawns = new List<Pawn>();
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
					_posOnBorders.Add(new Vector2I(i, j));
				}
			}
		}

		foreach (var (pos, size) in new []
		         {
			         (new Vector2I(0, Configuration.MapSize * Configuration.TileSize.Y / 2), new Vector2I(2, Configuration.MapSize * Configuration.TileSize.Y)),
			         (new Vector2I(Configuration.MapSize * Configuration.TileSize.X / 2, 0), new Vector2I(Configuration.MapSize * Configuration.TileSize.X, 2)),
			         (new Vector2I(Configuration.MapSize * Configuration.TileSize.X, Configuration.MapSize * Configuration.TileSize.Y / 2), new Vector2I(2, Configuration.MapSize * Configuration.TileSize.Y)),
			         (new Vector2I(Configuration.MapSize * Configuration.TileSize.X / 2, Configuration.MapSize * Configuration.TileSize.Y), new Vector2I(Configuration.MapSize * Configuration.TileSize.X, 2))
		         })
		{
			var shape = new RectangleShape2D();
			shape.Size = size;
			CollisionShape2D collisionShape = new CollisionShape2D();
			collisionShape.Shape = shape;
			collisionShape.Position = pos;
			_borderCollider.AddChild(collisionShape);
			// var shapeOwnerId = _borderCollider.CreateShapeOwner(shape);
			// _borderCollider.ShapeOwnerSetTransform(shapeOwnerId, new Transform2D(0, pos));
			// _borderCollider.ShapeOwnerAddShape(shapeOwnerId, shape);
		}
		
		
		// ((RectangleShape2D)_colliderShape.Shape).Size = new Vector2(Configuration.MapSize * Configuration.TileSize.X, 
		// 	Configuration.MapSize * Configuration.TileSize.Y);
		//
		//
		// _colliderShape.Position = new Vector2((float)Configuration.MapSize * Configuration.TileSize.Y / 2, 
		// 	(float)Configuration.MapSize * Configuration.TileSize.Y / 2);
		
		_rangeQueryCircleRid = PhysicsServer2D.CircleShapeCreate();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Clear()
	{
		foreach (var pawn in Pawns)
		{
			pawn.QueueFree();
		}
		Pawns.Clear();
	}
	
	public bool IsPosInBound(Vector2I pos)
	{
		return pos.X is >= 0 and < Configuration.MapSize && pos.Y is >= 0 and < Configuration.MapSize;
	}

	public void SpawnPawn(Pawn pawn, Vector2I pos)
	{
		Pawns.Add(pawn);
		AddChild(pawn);
		pawn.SetPos(pos);
	}
	
	public Vector2I RandomPosOnBorders(bool findUnoccupied = false)
	{
		Span<Vector2I> availablePos = stackalloc Vector2I[_posOnBorders.Count];
		var count = 0;
		foreach (var pos in _posOnBorders)
		{
			if (!findUnoccupied || !Tiles[pos.X, pos.Y].IsOccupied())
			{
				availablePos[count++] = pos;
			}
		}
		return availablePos[_gameMgr.RandomInt(count)];
	}

	public Vector2I RandomPosAround(Vector2I pos, float radius)
	{
		var count = 0;
		var tile = Tiles[pos.X, pos.Y];
		var collider = tile.Collider;
		var spaceState = GetWorld2D().DirectSpaceState;
		PhysicsServer2D.ShapeSetData(_rangeQueryCircleRid, radius);
		var query = new PhysicsShapeQueryParameters2D();
		query.ShapeRid = _rangeQueryCircleRid;
		query.Transform = tile.GetGlobalTransform();
		query.CollisionMask = collider.CollisionMask;
		query.Exclude = new Godot.Collections.Array<Rid>
		{
			collider.GetRid()
		};
		query.CollideWithAreas = true;
		var tilesInRadius = spaceState.IntersectShape(query);
		List<Vector2I> res = new List<Vector2I>();
		foreach (var collisionInfo in tilesInRadius)
		{
			if (collisionInfo["collider"].As<Area2D>() is Area2DWithRef { Controller: Tile tileInRadius })
			{
				var posInRadius = Utils.WorldToMap(tileInRadius.Position);
				if (posInRadius != pos)
				{
					res.Add(posInRadius);
				}
				res.Add(tileInRadius.MapPos);
				count += 1;
			}
		}
		return res[_gameMgr.RandomInt(count)];
	}

	// public void OnAreaExited(Area2D area)
	// {
	// 	GD.Print("OnAreaExited Map");
	// 	area.EmitSignal("area_exited", _collider);
	// }

	public Pawn GetPawnAt(Vector2I pos)
	{
		var tile = Tiles[pos.X, pos.Y];
		foreach (var overlappingArea in tile.Collider.GetOverlappingAreas())
		{
			if (overlappingArea is Area2DWithRef { Controller: Pawn pawn })
			{
				return pawn;
			}
		}
		return null;
	}

	public bool CanSetPosAt(Vector2I pos)
	{
		return GetPawnAt(pos) == null;
	}
}
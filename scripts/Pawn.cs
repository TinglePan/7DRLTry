using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using Proj7DRL.scripts.data_binding;

namespace Proj7DRL.scripts;

public partial class Pawn : Node2D
{
	[Export] public Area2D Collider;
	[Export] public Sprite2D Sprite;
	
	protected GameMgr GameMgr;
	protected ObservableProperty<Vector2I> MapPos;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameMgr = GetNode<GameMgr>("/root/GameMgr");
		MapPos = new ObservableProperty<Vector2I>("_mapPos", new Vector2I(-1, -1));
		MapPos.DetailedValueChanged += OnMapPosChanged;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public virtual void Stall()
	{
		GameMgr.PlayerTurnEnd();
	}

	public virtual void MoveByDir(FlagConstants.Direction dir)
	{
		if (!CollisionTest(dir)) return;
		var toPos = MapPos.Value + Dir2Dxy(dir);
		SetPos(toPos);
		GameMgr.PlayerTurnEnd();
	}

	public void SetPos(Vector2I toPos)
	{
		if (!GameMgr.Map.IsPosInBound(toPos)) return;
		MapPos.Value = toPos;
	}

	public void OnBodyEntered(Node2D body)
	{
		GD.Print("hit");
		GD.Print("body");
	}

	private void OnMapPosChanged(object sender, ValueChangedEventDetailedArgs<Vector2I> args)
	{
		var toMapPos = args.NewValue;
		var worldPos = Utils.MapToWorld(toMapPos);
		Position = worldPos;
	}

	protected Vector2I Dir2Dxy(FlagConstants.Direction dir)
	{
		int x, y;
		switch (dir & (FlagConstants.Direction.Down | FlagConstants.Direction.Up))
		{
			case 0:
				y = 0;
				break;
			case FlagConstants.Direction.Down:
				y = -1;
				break;
			case FlagConstants.Direction.Up:
				y = 1;
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(dir), dir, "Bitwise operation on FlagConstants.Direction is problematic");
		}
		if (Configuration.StartAtTopLeft)
		{
			y = -y;
		}
		switch (dir & (FlagConstants.Direction.Left | FlagConstants.Direction.Right))
		{
			case 0:
				x = 0;
				break;
			case FlagConstants.Direction.Left:
				x = -1;
				break;
			case FlagConstants.Direction.Right:
				x = 1;
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(dir), dir, "Bitwise operation on direction9 is problematic");
		}
		var dxy = new Vector2I(x, y);
		return dxy;
	}

	protected FlagConstants.Direction Dxy2Dir(Vector2I dxy)
	{
		FlagConstants.Direction dir = 0;
		if (dxy.X > 0)
		{
			dir |= FlagConstants.Direction.Right;
		}
		else if (dxy.X < 0)
		{
			dir |= FlagConstants.Direction.Left;
		}
		if (dxy.Y > 0)
		{
			dir |= FlagConstants.Direction.Up;
		} else if (dxy.Y < 0)
		{
			dir |= FlagConstants.Direction.Down;
		}
		return dir;
	}

	public bool CollisionTest(FlagConstants.Direction dir)
	{
		var mask = Collider.CollisionMask;
		var spaceState = GetWorld2D().DirectSpaceState;
		Vector2 worldDxy = Dir2Dxy(dir) * Configuration.TileSize;
		var globalTransform = GetGlobalTransform();
		var to = worldDxy + globalTransform.Origin;
		var query = PhysicsRayQueryParameters2D.Create(globalTransform.Origin, to, mask, new Array<Rid>
		{
			Collider.GetRid()
		});
		// Ouch.
		query.CollideWithAreas = true;
		var res = spaceState.IntersectRay(query);
		return res.Count == 0;
	}
}
using System;
using System.Collections.Generic;
using Godot;
using Proj7DRL.scripts.data_binding;

namespace Proj7DRL.scripts;

public partial class Pawn : Node2D
{
	[Export] private Sprite2D _centerPartSprite;
	[Export] private Sprite2D _topLeftPartSprite;
	[Export] private Sprite2D _topPartSprite;
	[Export] private Sprite2D _topRightPartSprite;
	[Export] private Sprite2D _rightPartSprite;
	[Export] private Sprite2D _bottomRightPartSprite;
	[Export] private Sprite2D _bottomPartSprite;
	[Export] private Sprite2D _bottomLeftPartSprite;
	[Export] private Sprite2D _leftPartSprite;

	private Dictionary<FlagConstants.Direction, Sprite2D> _dir2PartSpriteMap;
	
	private GameMgr _gameMgr;
	private ObservableProperty<Vector2I> _mapPos;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_dir2PartSpriteMap = new()
		{
			{ FlagConstants.Direction.UpLeft, _topLeftPartSprite },
			{ FlagConstants.Direction.Up, _topPartSprite },
			{ FlagConstants.Direction.UpRight, _topRightPartSprite },
			{ FlagConstants.Direction.Right, _rightPartSprite },
			{ FlagConstants.Direction.DownRight, _bottomRightPartSprite },
			{ FlagConstants.Direction.Down, _bottomPartSprite },
			{ FlagConstants.Direction.DownLeft, _bottomLeftPartSprite },
			{ FlagConstants.Direction.Left, _leftPartSprite },
			{ FlagConstants.Direction.Neutral, _centerPartSprite }
		};
		_gameMgr = GetNode<GameMgr>("/root/Entry/GameMgr");
		_mapPos = new ObservableProperty<Vector2I>("_mapPos", new Vector2I(-1, -1));
		_mapPos.DetailedValueChanged += OnMapPosChanged;
		UpdatePartSprites();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Stall()
	{
		_gameMgr.PlayerTurnEnd();
	}

	public void MoveByDir(FlagConstants.Direction dir)
	{
		var toPos = _mapPos.Value + Dir2Dxy(dir);
		SetPos(toPos);
		_gameMgr.PlayerTurnEnd();
	}

	public void SetPos(Vector2I toPos)
	{
		if (!_gameMgr.Map.IsPosInBound(toPos)) return;
		_mapPos.Value = toPos;
	}

	public void Rotate(IdConstants.RotateDirection dir)
	{
		
	}

	private void OnMapPosChanged(object sender, ValueChangedEventDetailedArgs<Vector2I> args)
	{
		var toMapPos = args.NewValue;
		var worldPos = Utils.MapToWorld(toMapPos);
		Position = worldPos;
	}

	private Vector2I Dir2Dxy(FlagConstants.Direction dir)
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

	private void UpdatePartSprites()
	{
		if (GetNodeOrNull<PlayerControl>("PlayerControl") is { } playerControl)
		{
			var controlPanel = _gameMgr.ControlPanel;
			foreach (var (dir, partSprite) in _dir2PartSpriteMap)
			{
				partSprite.Texture = controlPanel.GetTextureAtDir(dir);
			}
		} else if (GetNodeOrNull<Hostile> ("Hostile") is { } hostile)
		{
			foreach (var (dir, partSprite) in _dir2PartSpriteMap)
			{
				partSprite.Texture = hostile.GetTextureAtDir(dir);
			}
		}
	}
}
using System.Collections.Generic;
using Godot;

namespace Proj7DRL.scripts;

public partial class PlayerControl : Node
{
	
	private static Dictionary<IdConstants.CommandCode, FlagConstants.Direction> _commandCode2DirectionMap = new()
	{
		{ IdConstants.CommandCode.Stall, FlagConstants.Direction.Neutral },
		{ IdConstants.CommandCode.MoveUp, FlagConstants.Direction.Up },
		{ IdConstants.CommandCode.MoveRight, FlagConstants.Direction.Right },
		{ IdConstants.CommandCode.MoveDown, FlagConstants.Direction.Down },
		{ IdConstants.CommandCode.MoveLeft, FlagConstants.Direction.Left },
		{ IdConstants.CommandCode.MoveUpRight, FlagConstants.Direction.UpRight },
		{ IdConstants.CommandCode.MoveDownRight, FlagConstants.Direction.DownRight },
		{ IdConstants.CommandCode.MoveUpLeft, FlagConstants.Direction.UpLeft },
		{ IdConstants.CommandCode.MoveDownLeft, FlagConstants.Direction.DownLeft },
	};
	
	private static Dictionary<IdConstants.CommandCode, IdConstants.RotateDirection> _commandCode2RotateDirectionMap = new()
	{
		{ IdConstants.CommandCode.RotateClockwise, IdConstants.RotateDirection.Clockwise },
		{ IdConstants.CommandCode.RotateCounterClockwise, IdConstants.RotateDirection.CounterClockwise },
	};
	
	private bool _hasInitialized = false;
	
	private Pawn _parent;

	private GameMgr _gameMgr;

	private InputMgr _inputMgr;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_parent = GetParent<Pawn>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!_hasInitialized)
		{
			_gameMgr = GetNode("/root/Entry/GameMgr") as GameMgr;
			_inputMgr = GetNode("/root/Entry/InputMgr") as InputMgr;
			_hasInitialized = true;
			foreach (var (commandCode, direction) in _commandCode2DirectionMap)
			{
				_inputMgr.RegisterCommandHandler(commandCode, HandleDirectionCommand);
				// _inputMgr.RegisterCommandHandler(commandCode, HandleDirectionCommand, isHold:true);
			}

			foreach (var (commandCode, rotateDirection) in _commandCode2RotateDirectionMap)
			{
				_inputMgr.RegisterCommandHandler(commandCode, HandleRotateCommand);
			}
		}
	}
	
	private void HandleDirectionCommand(CommandHandlerArgs args)
	{
		var commandCode = args.CommandCode;
		if (!_commandCode2DirectionMap.TryGetValue(commandCode, out var direction)) return;
		if (direction == FlagConstants.Direction.Neutral)
		{
			_parent.Stall();
		}
		else
		{
			_parent.MoveByDir(direction);
		}
	}

	private void HandleRotateCommand(CommandHandlerArgs args)
	{
		var commandCode = args.CommandCode;
		if (!_commandCode2RotateDirectionMap.TryGetValue(commandCode, out var rotateDirection)) return;
		_parent.Rotate(rotateDirection);
	}
}
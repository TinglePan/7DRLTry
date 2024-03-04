using System.Collections.Generic;
using Godot;

namespace Proj7DRL.scripts;

public struct CommandHandlerArgs
{
    public IdConstants.CommandCode CommandCode;
    public ulong HoldTime;
}

public partial class InputMgr: Node
{
    [Export]
    public float HoldInputTime = 0.5f;
    
    private static readonly Dictionary<string, IdConstants.CommandCode> CommandCodeMap = new()
    {
        {"Stall", IdConstants.CommandCode.Stall},
        {"MoveUpLeft", IdConstants.CommandCode.MoveUpLeft},
        {"MoveUpRight", IdConstants.CommandCode.MoveUpRight},
        {"MoveDownLeft", IdConstants.CommandCode.MoveDownLeft},
        {"MoveDownRight", IdConstants.CommandCode.MoveDownRight},
        {"MoveRight", IdConstants.CommandCode.MoveRight},
        {"MoveLeft", IdConstants.CommandCode.MoveLeft},
        {"MoveUp", IdConstants.CommandCode.MoveUp},
        {"MoveDown", IdConstants.CommandCode.MoveDown},
        {"RotateClockwise", IdConstants.CommandCode.RotateClockwise},
        {"RotateCounterClockwise", IdConstants.CommandCode.RotateCounterClockwise}
    };
    
    private Dictionary<IdConstants.CommandCode, CommandHandler> _onAction;
    private Dictionary<IdConstants.CommandCode, CommandHandler> _onActionHold;
    private Dictionary<IdConstants.CommandCode, ulong> _holdStartTime;
    
    public delegate void CommandHandler(CommandHandlerArgs args);

    public override void _Ready()
    {
        _onAction = new Dictionary<IdConstants.CommandCode, CommandHandler>();
        _onActionHold = new Dictionary<IdConstants.CommandCode, CommandHandler>();
        _holdStartTime = new Dictionary<IdConstants.CommandCode, ulong>();
    }
    
    public override void _Input(InputEvent @event)
    {
        var currTime = Time.GetTicksMsec();
        foreach (var (eventName, commandCode) in CommandCodeMap)
        {
            if (@event.IsActionPressed(eventName))
            {
                _onAction.GetValueOrDefault(commandCode)?.Invoke(new CommandHandlerArgs { CommandCode = commandCode });
                _holdStartTime[commandCode] = currTime;
            }
            if (@event.IsActionReleased(eventName))
            {
                _holdStartTime.Remove(commandCode);
            }
        }
    }
    public override void _PhysicsProcess(double delta)
    {
        var currTime = Time.GetTicksMsec();
        foreach (var (commandCode, holdStartTime) in _holdStartTime)
        {
            var holdTime = currTime - holdStartTime;
            if (holdTime > HoldInputTime)
            {
                _onActionHold.GetValueOrDefault(commandCode)?.Invoke(new CommandHandlerArgs
                {
                    CommandCode = commandCode,
                    HoldTime = holdTime
                });
            }
        }
    }
    
    public void RegisterCommandHandler(IdConstants.CommandCode commandCode, CommandHandler handler, bool isHold=false)
    {
        var target = isHold ? _onActionHold : _onAction;
        if (!target.TryAdd(commandCode, handler))
        {
            target[commandCode] += handler;
        }
    }
    
    public void UnregisterCommandHandler(IdConstants.CommandCode commandCode, CommandHandler handler, bool isHold=false)
    {
        var target = isHold ? _onActionHold : _onAction;
        if (!target.ContainsKey(commandCode)) return;
        target[commandCode] -= handler;
        if (target[commandCode] == null)
        {
            target.Remove(commandCode);
        }
    }
}
using System.Collections.Generic;
using Godot;

namespace Proj7DRL.scripts;

public struct CommandHandlerArgs
{
    public IdConstants.CommandCode CommandCode;
    public ulong HoldTime;
}

public class InputMgr
{
    private static readonly Dictionary<string, IdConstants.CommandCode> CommandCodeMap = new()
    {
        {"Stall", IdConstants.CommandCode.Stall},
        {"UpLeft", IdConstants.CommandCode.MoveUpLeft},
        {"UpRight", IdConstants.CommandCode.MoveUpRight},
        {"DownLeft", IdConstants.CommandCode.MoveDownLeft},
        {"DownRight", IdConstants.CommandCode.MoveDownRight},
        {"Right", IdConstants.CommandCode.MoveRight},
        {"Left", IdConstants.CommandCode.MoveLeft},
        {"Up", IdConstants.CommandCode.MoveUp},
        {"Down", IdConstants.CommandCode.MoveDown},
    };
    
    private GameMgr _gameMgr;
    
    private Dictionary<IdConstants.CommandCode, CommandHandler> _onAction;
    private Dictionary<IdConstants.CommandCode, CommandHandler> _onActionHold;
    private Dictionary<IdConstants.CommandCode, ulong> _holdStartTime;
    
    public delegate void CommandHandler(CommandHandlerArgs args);
    
    public InputMgr(GameMgr gameMgr)
    {
        _gameMgr = gameMgr;
        _onAction = new Dictionary<IdConstants.CommandCode, CommandHandler>();
        _onActionHold = new Dictionary<IdConstants.CommandCode, CommandHandler>();
        _holdStartTime = new Dictionary<IdConstants.CommandCode, ulong>();
    }
    
    public void OnInput(InputEvent @event)
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

    public void OnPhysicsProcess(float delta)
    {
        var currTime = Time.GetTicksMsec();
        foreach (var (commandCode, holdStartTime) in _holdStartTime)
        {
            var holdTime = currTime - holdStartTime;
            if (holdTime > Configuration.HoldInputTime)
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
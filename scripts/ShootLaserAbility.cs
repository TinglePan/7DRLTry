using System.Collections.Generic;
using Godot;

namespace Proj7DRL.scripts;

public class ShootLaserAbility: Ability
{
    public int Power;
    public int DotPower;
    public int Cooldown;
    public int Duration;
    
    public List<FlagConstants.Direction> Directions;
    
    private int _cooldownCounter;
    private int _durationCounter;
    private Dictionary<FlagConstants.Direction, BaseProjectile> _lasers;
    
    public ShootLaserAbility(int id, GameMgr gameMgr) : base(id, gameMgr)
    {
        _cooldownCounter = 0;
        _durationCounter = 0;
        _lasers = new Dictionary<FlagConstants.Direction, BaseProjectile>();
        Power = (int)Def.Args["power"];
        DotPower = (int)Def.Args["dotPower"];
        Cooldown = (int)Def.Args["cooldown"];
        Directions = new List<FlagConstants.Direction>();
        Duration = (int)Def.Args["duration"];
        foreach (var dir in (Godot.Collections.Array)Def.Args["directions"])
        {
            Directions.Add((FlagConstants.Direction)(int)dir);
        }
    }

    public override void OnMove()
    {
        base.OnMove();
        Stop();
    }

    public override void OnRotate()
    {
        base.OnRotate();
        Stop();
    }

    public override void OnStall()
    {
        base.OnStall();
        if (_durationCounter > 0)
        {
            _durationCounter--;
            if (_durationCounter <= 0)
            {
                Stop();
            }
        }
        if (_cooldownCounter <= 0)
        {
            foreach (var direction in Directions)
            {
                var laser = GameMgr.PlayerPawn.Shoot(ProjectileType.Laser, new Dictionary<string, Variant>()
                {
                    { "startPos", GameMgr.PlayerPawn.MapPos.Value },
                    { "power", Power },
                    { "dotPower", DotPower },
                    { "source", GameMgr.PlayerPawn },
                    { "dirVec", GameMgr.PlayerPawn.Dir2Dxy(Utils.RotateDirByDir(direction, GameMgr.PlayerPawn.FaceDirection)) },
                    { "width", 32.0 },
                    { "distance", 32.0 },
                });
                _lasers.Add(direction, laser);
            }
            _durationCounter = Duration;
            _cooldownCounter = Cooldown;
        }
    }

    public override void OnAction()
    {
        if (_cooldownCounter > 0)
        {
            _cooldownCounter--;
        }
    }

    private void Stop()
    {
        _durationCounter = 0;
        foreach (var laser in _lasers.Values)
        {
            GameMgr.DestroyProjectile(laser);
        }
        _lasers.Clear();
    }
}
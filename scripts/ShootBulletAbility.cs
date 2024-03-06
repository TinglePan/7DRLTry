using System.Collections.Generic;
using Godot.Collections;

namespace Proj7DRL.scripts;

public class ShootBulletAbility: Ability
{
    public int Power;
    public int Cooldown;
    public List<FlagConstants.Direction> Directions;
    
    private int _cooldownCounter;
    
    public ShootBulletAbility(int id, GameMgr gameMgr) : base(id, gameMgr)
    {
        _cooldownCounter = 0;
        Power = (int)Def.Args["power"];
        Cooldown = (int)Def.Args["cooldown"];
        Directions = new List<FlagConstants.Direction>();
        foreach (var dir in (Array)Def.Args["directions"])
        {
            Directions.Add((FlagConstants.Direction)(int)dir);
        }
    }

    public override void OnMove()
    {
        if (_cooldownCounter > 0)
        {
            _cooldownCounter--;
        }
        else
        {
            GameMgr.PlayerPawn.Shoot(ProjectileType.Bullet, Directions, Power);
            _cooldownCounter = Cooldown;
        }
    }
}
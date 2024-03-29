using System.Collections.Generic;
using Godot;
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
        base.OnMove();
        if (_cooldownCounter > 0)
        {
            _cooldownCounter--;
        }
        else
        {
            foreach (var direction in Directions)
            {
                GameMgr.PlayerPawn.Shoot(ProjectileType.Bullet, new System.Collections.Generic.Dictionary<string, Variant>()
                {
                    { "startPos", GameMgr.PlayerPawn.MapPos.Value },
                    { "power", Power },
                    { "source", GameMgr.PlayerPawn },
                    { "dirVec", GameMgr.PlayerPawn.Dir2Dxy(Utils.RotateDirByDir(direction, GameMgr.PlayerPawn.FaceDirection))}
                });
            }
            _cooldownCounter = Cooldown;
        }
    }
}
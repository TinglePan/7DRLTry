using System;
using System.Collections.Generic;
using Godot;
using Proj7DRL.scripts.enemy_ai;

namespace Proj7DRL.scripts;

public partial class HostilePawn: Pawn
{

    private EnemyDef _def;
    public BaseEnemyAi Ai;
    
    // public override void _Ready()
    // {
    //     base._Ready();
    // }

    public void Setup(EnemyDef def)
    {
        _def = def;
        InnerHp.Value = def.Hp;
        Ai = GetNode<BaseEnemyAi>("Ai");
    }

    public override void _Process(double delta)
    {
        
    }

    public void Retreat()
    {
        GD.Print("Retreat HostilePawn");
        GameMgr.DestroyPawn(this);
    }

    public void UseAbility(EnemyAbilities abilityId, Dictionary<string, Variant> args)
    {
        int rand;
        Vector2 dirVec;
        switch (abilityId)
        {
            case EnemyAbilities.Bump:
                var target = args["target"].As<Pawn>();
                var power = args["power"].AsInt32();
                target.TakeDamage(this, power);
                break;
            case EnemyAbilities.ShootBullet:
                Shoot(args["bulletType"].As<ProjectileType>(), args);
                break;
            case EnemyAbilities.ShootBulletPot:
                var chance = args["chance"].AsInt32();
                foreach (FlagConstants.Direction testDir in Enum.GetValues(typeof(FlagConstants.Direction)))
                {
                    rand = GameMgr.Rand.Next(100);
                    if (rand <= chance)
                    {
                        dirVec = Dir2Dxy(testDir);
                        args["dirVec"] = dirVec;
                        UseAbility(EnemyAbilities.ShootBullet, args);
                    }
                }
                break;
            case EnemyAbilities.ShootBulletAimTarget:
                var acc = args["accuracy"].AsInt32();
                var spread = args["spread"].As<float>();
                rand = GameMgr.Rand.Next(100);
                Vector2I targetPos;
                if (rand <= acc)
                {
                    targetPos = args["targetPos"].AsVector2I();
                }
                else
                {
                    targetPos = GameMgr.Map.RandomPosAround(args["targetPos"].AsVector2I(), spread);
                }
                args["targetPos"] = targetPos;
                UseAbility(EnemyAbilities.ShootBulletAtTarget, args);
                break;
            case EnemyAbilities.ShootBulletAtTarget:
                dirVec = args["targetPos"].AsVector2I() - MapPos.Value;
                args["dirVec"] = dirVec;
                UseAbility(EnemyAbilities.ShootBullet, args);
                break;
            case EnemyAbilities.Explode:
                // NYI
                break;
        }
    }
}
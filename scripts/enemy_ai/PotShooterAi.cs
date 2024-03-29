using System;
using System.Collections.Generic;
using Godot;
using SadRogue.Primitives;
using ShaiRandom.Distributions;

namespace Proj7DRL.scripts.enemy_ai;

public partial class PotShooterAi: BaseLineMoverAi
{
    // enum State
    // {
    //     Moving,
    //     Settled
    // }
    
    // private State _state;

    private int shootAbilityCooldown = 0;
    
    public override void Setup(Dictionary<string, Variant> args)
    {
        base.Setup(args);
        // _state = Dir == Vector2I.Zero ? State.Settled : State.Moving;
    }
    
    public override void Proceed()
    {
        // switch (_state)
        // {
        //     case State.Moving:
        //         var rand = GameMgr.Rand.Next(100);
        //         if (rand <= 50)
        //         {
        //             ProceedMove();
        //         }
        //         else
        //         {
        //             _state = State.Settled;
        //         }
        //         break;
        //     case State.Settled:
        //         Pawn.UseAbility(EnemyAbilities.ShootBulletPot, new Dictionary<string, Variant>
        //         {
        //         });
        //         break;
        // }
        if (shootAbilityCooldown <= 0)
        {
            Pawn.UseAbility(EnemyAbilities.ShootBulletPot, new Dictionary<string, Variant>
            {
                { "bulletType", (int)ProjectileType.Bullet },
                { "power", 2 },
                { "chance", 35 }
            });
            shootAbilityCooldown = 2;
        }
        ProceedMove();
        shootAbilityCooldown--;
    }

    protected override void MoveBlocked(Pawn blocker)
    {
        // base.MoveBlocked(blocker);
        // _state = State.Settled;
        base.MoveBlocked(blocker);
        Pawn.UseAbility(EnemyAbilities.Bump, new Dictionary<string, Variant>()
        {
            { "target", blocker },
            { "power", 1 },
        });
        shootAbilityCooldown = 2;

    }
}
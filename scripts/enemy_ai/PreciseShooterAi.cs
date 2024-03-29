using System.Collections.Generic;
using Godot;

namespace Proj7DRL.scripts.enemy_ai;

public partial class PreciseShooterAi: BaseLineMoverAi
{

    enum State
    {
        Moving,
        Settled,
    }
    
    private PlayerPawn _target;
    private State _state;

    public override void _Ready()
    {
        base._Ready();
        _target = GameMgr.PlayerPawn;
        _state = State.Moving;
    }

    public override void Proceed()
    {
        switch (_state)
        {
            case State.Moving:
                var rand = GameMgr.Rand.Next(100);
                if (rand <= 50)
                {
                    ProceedMove();
                }
                else
                {
                    _state = State.Settled;
                }
                break;
            case State.Settled:
                Pawn.UseAbility(EnemyAbilities.ShootBulletAtTarget, new Dictionary<string, Variant>
                {
                    { "bulletType", (int)ProjectileType.Bullet },
                    { "targetPos", _target.MapPos.Value },
                    { "power", 2 },
                    { "spread", 1 },
                    { "accuracy", 70 }
                });
                break;
        }
    }
    
    protected override void MoveBlocked(Pawn blocker)
    {
        base.MoveBlocked(blocker);
        _state = State.Settled;
    }
}
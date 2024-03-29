using System.Collections.Generic;
using Godot;

namespace Proj7DRL.scripts;

public partial class BulletProjectile: BaseProjectile
{
    [Export] public float Speed = 1000;
    
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        Position += Transform.Y * (float)delta * Speed;
    }

    public override void Setup(Dictionary<string, Variant> args)
    {
        base.Setup(args);
        RegisterWait();
    }
    
    protected override void OnHitPawn(Pawn pawn)
    {
        base.OnHitPawn(pawn);
        if (pawn != Source)
        {
            pawn.TakeDamage(Source, Power);
            GameMgr.DestroyProjectile(this);
        }
    }

    protected override void OnHitProjectile(BaseProjectile projectile)
    {
        if (Source != projectile.Source)
        {
            base.OnHitProjectile(projectile);
            GameMgr.DestroyProjectile(this);
        }
    }
}
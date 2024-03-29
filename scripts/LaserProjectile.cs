using System.Collections.Generic;
using Godot;

namespace Proj7DRL.scripts;

public partial class LaserProjectile: BaseProjectile
{
    [Export] public CollisionShape2D CollisionShape;
    
    public float Distance;
    public float Width;

    private float _actualDistance;
    
    public override void Setup(Dictionary<string, Variant> args)
    {
        base.Setup(args);
        var startPos = Utils.WorldToMap(Position);
        Distance = args["distance"].As<float>();
        _actualDistance = Distance;
        Width = args["width"].As<float>();
        RegisterWait();
        Cast();
    }
    
    protected override void OnHitProjectile(BaseProjectile projectile)
    {
        if (projectile is BulletProjectile bullet)
        {
            GameMgr.DestroyProjectile(bullet);
        }
    }

    private void Cast()
    {
        var mask = Collider.CollisionMask;
        var spaceState = GetWorld2D().DirectSpaceState;
        var globalTransform = GetGlobalTransform();
        var to = globalTransform.Origin + globalTransform.BasisXform(new Vector2(0, Distance));
        var query = PhysicsRayQueryParameters2D.Create(globalTransform.Origin, to, mask, new Godot.Collections.Array<Rid>
        {
            Collider.GetRid()
        });
        query.CollideWithAreas = true;
        var hit = spaceState.IntersectRay(query);
        if (hit.Count != 0)
        {
            var collisionPos = hit["position"].AsVector2();
            _actualDistance = GlobalPosition.DistanceTo(collisionPos);
        }

        Position = Source.Position + Source.GlobalTransform.BasisXform(Vector2.Up) * _actualDistance / 2;
        ((RectangleShape2D)CollisionShape.Shape).Size = new Vector2(Width, _actualDistance);
        Sprite.Scale = new Vector2(Width / Sprite.Texture.GetWidth(), _actualDistance / Sprite.Texture.GetHeight());
    }

}
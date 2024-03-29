using System.Collections.Generic;
using Godot;

namespace Proj7DRL.scripts;

public enum ProjectileType
{
    Bullet,
    Missile,
    Laser
}

public partial class BaseProjectile: Node2D
{
    [Export] public Area2D Collider;
    [Export] public Sprite2D Sprite;
    
    public string DisplayName;
    public string Description;
    public int Power;
    
    public Pawn Source;
    
    protected GameMgr GameMgr;

    public override void _Ready()
    {
        GameMgr = GetNode<GameMgr>("/root/GameMgr");
    }

    public virtual void Setup(Dictionary<string, Variant> args)
    {
        var startPos = args["startPos"].As<Vector2I>();
        Position = Utils.MapToWorld(startPos);
        if (args.ContainsKey("rotationDegrees"))
        {
            RotationDegrees = args["rotationDegrees"].As<float>();
        } else if (args.ContainsKey("rotation"))
        {
            Rotation = args["rotation"].As<float>();
        }
        Power = args["power"].As<int>();
        Source = args["source"].As<Pawn>();
    }
    
    public virtual void OnAreaEntered(Area2D area)
    {
        GD.Print("OnAreaEnter Projectile");
        if (area is Area2DWithRef areaWithRef)
        {
            if (areaWithRef.Controller is Pawn pawn && pawn != Source)
            {
                GD.Print("hit pawn");
                OnHitPawn(pawn);
            } else if (areaWithRef.Controller is BaseProjectile projectile && projectile != this)
            {
                GD.Print("hit projectile");
                OnHitProjectile(projectile);
            } else if (areaWithRef.Controller is Map map)
            {
                GD.Print("hit map border");
                OnHitMapBorder();
            }
        }
    }

    // public virtual void OnAreaExited(Area2D area)
    // {
    //     GD.Print("OnAreaExited Projectile");
    // }

    public void RegisterWait()
    {
        GameMgr.WaitingProjectiles.Add(this);
    }

    public void RegisterDoTCheck()
    {
        GameMgr.DoTProjectiles.Add(this);
    }

    public virtual void OnDoTCheck()
    {
        GD.Print($"dot check for {this}");
    }

    protected virtual void OnHitPawn(Pawn pawn)
    {
        GD.Print($"hit pawn {pawn}");
    }

    protected virtual void OnHitProjectile(BaseProjectile projectile)
    {
        GD.Print($"hit projectile {projectile}");
    }

    protected virtual void OnHitMapBorder()
    {
        GameMgr.DestroyProjectile(this);
    }
}
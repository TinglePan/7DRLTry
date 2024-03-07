using System.Collections.Generic;
using Godot;

namespace Proj7DRL.scripts;

public enum ProjectileType
{
    Bullet,
    Laser
} 

public partial class Projectile: Node2D
{
    [Export] public Area2D Collider;
    [Export] public float _speed;
    
    public int Power;
    private Vector2 _dir;
    
    private List<Pawn> _hitPawns = new List<Pawn>();

    public void Setup(Vector2 pos, Vector2 dir, int power)
    {
        Position = pos;
        _dir = dir;
        Power = power;
    }

    public override void _PhysicsProcess(double delta)
    {
        var velocity = _dir * _speed;
        Position += velocity * (float)delta;
    }
    
    public void OnAreaEntered(Area2D area)
    {
        QueueFree();
    }
}
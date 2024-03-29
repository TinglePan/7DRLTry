﻿using System.Collections.Generic;
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

    public void Setup(Vector2 pos, float rotationDegrees, Vector2 dir, int power)
    {
        Position = pos;
        RotationDegrees = rotationDegrees;
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
        GD.Print("OnAreaEnter Projectile");
        if (area.Name != "MapBB")
        {
            QueueFree();
        }
    }

    public void OnAreaExited(Area2D area)
    {
        GD.Print("OnAreaExited Projectile");
        if (area.Name == "MapBB")
        {
            QueueFree();
        }
    }
}
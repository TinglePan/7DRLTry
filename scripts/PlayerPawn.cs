using System.Collections.Generic;
using System.Data;
using Godot;

namespace Proj7DRL.scripts;

public partial class PlayerPawn: Pawn
{
    [Export] private PackedScene _bulletPrefab;
    public int Power;
    public int Speed;
    public FlagConstants.Direction FaceDirection;

    public override void _Ready()
    {
        base._Ready();
        FaceDirection = FlagConstants.Direction.Up;
        Power = 1;
        Speed = 1;
    }
    
    public void Shoot(ProjectileType type, List<FlagConstants.Direction> directions, int power)
    {
        foreach (var direction in directions)
        {
            var faceDirVector = Dir2Dxy(FaceDirection);
            var relativeDirVector = Dir2Dxy(direction);
            var shootDirVector = Utils.RotateDirVectorByDirVector(faceDirVector, relativeDirVector);
            var bullet = _bulletPrefab.Instantiate<Projectile>();
            bullet.Setup(Position, shootDirVector, power);
            bullet.Collider.SetCollisionLayerValue(4, true);
            bullet.Collider.SetCollisionMaskValue(2, true);
            GameMgr.Map.AddChild(bullet);
        }
    }
}
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

    public override void Stall()
    {
        GameMgr.AbilityPanel.OnPlayerStall();
        base.Stall();
    }

    public override void MoveByDir(FlagConstants.Direction dir)
    {
        if (!CollisionTest(dir)) return;
        var toPos = MapPos.Value + Dir2Dxy(dir);
        SetPos(toPos);
        GameMgr.AbilityPanel.OnPlayerMove();
        GameMgr.PlayerTurnEnd();
    }

    public void Rotate(ClockDirection dir)
    {
        FaceDirection = Utils.RotateDirByClockDir(FaceDirection, dir);
        RotationDegrees = Utils.Dir2Angle(FaceDirection);
        GameMgr.AbilityPanel.OnPlayerRotate();
		GameMgr.PlayerTurnEnd();
    }
    
    public void Shoot(ProjectileType type, List<FlagConstants.Direction> directions, int power)
    {
        foreach (var direction in directions)
        {
            var shootDir = Utils.RotateDirByDir(FaceDirection, direction);
            var shootDirVector = Dir2Dxy(shootDir);
            var bullet = _bulletPrefab.Instantiate<Projectile>();
            bullet.Setup(Position, RotationDegrees, shootDirVector, power);
            bullet.Collider.SetCollisionLayerValue(3, true);
            bullet.Collider.SetCollisionMaskValue(2, true);
            GameMgr.Map.AddChild(bullet);
        }
    }
}
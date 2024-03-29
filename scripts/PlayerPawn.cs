using System.Collections.Generic;
using Godot;
using Proj7DRL.scripts.data_binding;

namespace Proj7DRL.scripts;

public partial class PlayerPawn: Pawn
{
    [Export] private PackedScene _bulletPrefab;
    public int Power;
    public FlagConstants.Direction FaceDirection;
    public ObservableProperty<int> MaxHp;

    public override void _Ready()
    {
        base._Ready();
        MaxHp = new ObservableProperty<int>("MaxHp", 0);
        FaceDirection = FlagConstants.Direction.Up;
        Power = 1;
    }

    public override void Setup(Dictionary<string, Variant> args)
    {
        InnerHp.Value = 20;
        MaxHp.Value = 20;
    }

    public override void Stall()
    {
        base.Stall();
        GameMgr.AbilityPanel.OnPlayerStall();
        GameMgr.PlayerTurnEnd();
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
    
    // public void Shoot(ProjectileType type, List<FlagConstants.Direction> directions, int power)
    // {
    //     foreach (var direction in directions)
    //     {
    //         var shootDir = Utils.RotateDirByDir(FaceDirection, direction);
    //         var shootDirVector = Dir2Dxy(shootDir);
    //         var bullet = _bulletPrefab.Instantiate<BulletProjectile>();
    //         bullet.Setup(new Dictionary<string, Variant>()
    //         {
    //             { "startPos", MapPos.Value },
    //             { "power", power },
    //             { "source", this }
    //         });
    //         bullet.Collider.SetCollisionLayerValue(3, true);
    //         bullet.Collider.SetCollisionMaskValue(2, true);
    //         GameMgr.Map.AddChild(bullet);
    //     }
    // }
}
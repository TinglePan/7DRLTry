using System.Collections.Generic;
using Godot;

namespace Proj7DRL.scripts.enemy_ai;

public partial class BaseLineMoverAi: BaseEnemyAi, ILineMove
{
    public Vector2 WorldPos { get; private set; }
    public Vector2 Dir { get; private set; }
    public float Step { get; private set; }

    public override void Setup(Dictionary<string, Variant> args)
    {
        base.Setup(args);
        WorldPos = Pawn.Position;
        var targetPos = args["targetPos"].AsVector2I();
        Step = args.ContainsKey("step") ? args["step"].AsInt32() : (1 / Mathf.Sqrt2);
        Dir = targetPos - Utils.WorldToMap(WorldPos);
    }
    
    public virtual void ProceedMove()
    {
        var previousPos = Utils.WorldToMap(WorldPos);
        WorldPos += Dir.Normalized() * Step * Configuration.TileSize.Length() ;
        Vector2I nextPos = Utils.WorldToMap(WorldPos);
        if (previousPos == nextPos)
        {
            GD.Print($"{Pawn} previousPos == nextPos");
            return;
        }
        if (!GameMgr.Map.IsPosInBound(nextPos))
        {
            Pawn.Retreat();
            return;
        }
        var pawnAtNextPos = GameMgr.Map.GetPawnAt(nextPos);
        switch (pawnAtNextPos)
        {
            case null:
                Pawn.SetPos(nextPos);
                break;
            case var pawn:
                MoveBlocked(pawn);
                break;
        }
    }

    protected virtual void MoveBlocked(Pawn blocker)
    {
        GD.Print($"move blocked by {blocker}");
    }
    
}
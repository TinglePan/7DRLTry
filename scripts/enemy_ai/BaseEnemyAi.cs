using System.Collections.Generic;
using Godot;

namespace Proj7DRL.scripts.enemy_ai;

public partial class BaseEnemyAi: Node2D
{

    protected GameMgr GameMgr;
    protected HostilePawn Pawn;
    protected Map Map;

    public override void _Ready()
    {
        GameMgr = GetNode<GameMgr>("/root/GameMgr");
        Map = GameMgr.Map;
    }

    public virtual void Setup(Dictionary<string, Variant> args)
    {
        var pawn = args["pawn"].As<HostilePawn>();
        Pawn = pawn;
    }
    
    public virtual void Proceed()
    {
        
    }
}
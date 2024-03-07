using Godot;
using Godot.Collections;

namespace Proj7DRL.scripts;

public class Ability
{
    protected AbilityDef Def;
    protected GameMgr GameMgr;
    public int Id;

    public string DisplayName => Def.DisplayName;
    public string Description => Def.Description;
    public string IconPath => Def.IconPath;
    
    public Ability(int id, GameMgr gameMgr)
    {
        Id = id;
        Def = AbilityDefs.GetDef(id);
        GameMgr = gameMgr;
    }

    public virtual void OnGetAbility()
    {
    }

    public virtual void OnMove()
    {
    }

    public virtual void OnStall()
    {
        
    }

    public virtual void OnRotate()
    {
        
    }

    public virtual void OnHit()
    {
    }
}
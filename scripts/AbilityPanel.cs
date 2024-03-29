using System.Collections.Generic;
using System.Xml.Xsl;
using Godot;

namespace Proj7DRL.scripts;

public partial class AbilityPanel: Control
{
    [Export]
    private PackedScene _slotPrefab;

    [Export] private PackedScene _iconPrefab;
    [Export]
    private GridContainer _gridContainer;

    [Export] private int _columns;

    private GameMgr _gameMgr;
    private List<Ability> _abilities = new();
    
    public override void _Ready()
    {
        _gameMgr = GetNode<GameMgr>("/root/GameMgr");
        _abilities = new List<Ability>();
        _gridContainer.Columns = _columns;
    }
    
    public void AddAbility(int id)
    {
        var def = AbilityDefs.GetDef(id);
        Ability ability;
        switch (def.Type)
        {
            case "ShootBulletAbility":
                ability = new ShootBulletAbility(id, _gameMgr);
                break;
            case "ShootLaserAbility":
                ability = new ShootLaserAbility(id, _gameMgr);
                break;
            default:
                ability = new Ability(id, _gameMgr);
                break;
        }
        _abilities.Add(ability);
        ability.OnGetAbility();
        var icon = _iconPrefab.Instantiate<AbilityIcon>();
        icon.Setup(ability, _gameMgr.InspectorLabel);
        _gridContainer.AddChild(icon);
    }

    public void Clear()
    {
        foreach (var childNode in _gridContainer.GetChildren())
        {
            childNode.QueueFree();
        }
        _abilities.Clear();
    }

    public void OnPlayerMove()
    {
        foreach (var ability in _abilities)
        {
            ability.OnMove();
        }
    }
    
    public void OnPlayerRotate()
    {
        foreach (var ability in _abilities)
        {
            ability.OnRotate();
        }
    }

    public void OnPlayerStall()
    {
        foreach (var ability in _abilities)
        {
            ability.OnStall();
        }
    }
}
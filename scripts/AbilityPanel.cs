using System.Collections.Generic;
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
        var ability = new Ability(id, _gameMgr);
        _abilities.Add(ability);
        ability.OnGetAbility();
        var icon = _iconPrefab.Instantiate<AbilityIcon>();
        icon.Texture = GD.Load<Texture2D>(ability.IconPath);
        _gridContainer.AddChild(icon);
    }
}
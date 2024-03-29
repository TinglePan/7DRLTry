using System.Collections.Generic;
using Godot;

namespace Proj7DRL.scripts;

public partial class Shop : Control
{
    [Export] private PackedScene _abilityIconPrefab;
    [Export] private HBoxContainer _abilityIconsContainer;
    [Export] private TextureButton _closeButton;

    public List<ShopAbilityIcon> AbilityIcons;

    private int _nAbilities;

    public override void _Ready()
    {
        AbilityIcons = new List<ShopAbilityIcon>();
    }

    public void Setup(List<Ability> abilities, AbilityPanel abilityPanel, Label inspectorLabel, int nAbilities)
    {
        _nAbilities = nAbilities;
        var nChildren = AbilityIcons.Count;
        for (int i = 0; i < abilities.Count; i++)
        {
            var ability = abilities[i];
            if (i < nChildren)
            {
                var icon = AbilityIcons[i];
                icon.Setup(ability, abilityPanel, inspectorLabel);
            }
            else
            {
                var icon = _abilityIconPrefab.Instantiate<ShopAbilityIcon>();
                icon.Setup(ability, abilityPanel, inspectorLabel);
                _abilityIconsContainer.AddChild(icon);
                AbilityIcons.Add(icon);
            }
        }

        _closeButton.Pressed += Close;
    }

    public void Close()
    {
        GD.Print("close");
        Hide();
        SetProcess(false);
    }

    public void Open()
    {
        Show();
        SetProcess(true);
    }

}
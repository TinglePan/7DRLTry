using System;
using Godot;

namespace Proj7DRL.scripts;

public partial class ShopAbilityIcon : TextureButton
{
	private Label _inspectorLabel;
	private AbilityPanel _abilityPanel;
	private Ability _ability;

	public void Setup(Ability ability, AbilityPanel abilityPanel, Label inspectorLabel)
	{
		TextureNormal = GD.Load<Texture2D>(ability.IconPath);
		_abilityPanel = abilityPanel;
		_inspectorLabel = inspectorLabel;
		_ability = ability;
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MouseEntered += () =>
		{
			_inspectorLabel.Text = _ability.Description;
		};
		MouseExited += () =>
		{
			_inspectorLabel.Text = "";
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Pressed()
	{
		_abilityPanel.AddAbility(_ability.Id);
	}
}
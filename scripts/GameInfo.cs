using System.Runtime.Loader;
using Godot;
using Proj7DRL.scripts.data_binding;

namespace Proj7DRL.scripts;

public partial class GameInfo : Node
{
	[Export] private ProgressBar _hpBar;
	[Export] private Label _hpValueLabel;
	[Export] private Label _maxHpValueLabel;
	[Export] private Label _scrapValueLabel;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Setup(GameMgr gameMgr)
	{
		var playerPawn = gameMgr.PlayerPawn;
		_hpBar.Value = playerPawn.Hp;
		_hpBar.MaxValue = playerPawn.MaxHp.Value;
		_hpValueLabel.Text = playerPawn.Hp.ToString();
		_maxHpValueLabel.Text = playerPawn.MaxHp.Value.ToString();
		_scrapValueLabel.Text = gameMgr.ScrapCount.Value.ToString();
		playerPawn.InnerHp.DetailedValueChanged += OnPlayerHpChanged;
		playerPawn.MaxHp.DetailedValueChanged += OnPlayerMaxHpChanged;
		gameMgr.ScrapCount.DetailedValueChanged += OnScrapCountChanged;
	}

	private void OnPlayerHpChanged(object sender, ValueChangedEventDetailedArgs<int> args)
	{
		_hpBar.Value = args.NewValue;
		_hpValueLabel.Text = args.NewValue.ToString();
	}

	private void OnPlayerMaxHpChanged(object sender, ValueChangedEventDetailedArgs<int> args)
	{
		_hpBar.MaxValue = args.NewValue;
		_maxHpValueLabel.Text = args.NewValue.ToString();
	}

	private void OnScrapCountChanged(object sender, ValueChangedEventDetailedArgs<int> args)
	{
		_scrapValueLabel.Text = args.NewValue.ToString();
	}
}
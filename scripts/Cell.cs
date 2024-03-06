using Godot;

namespace Proj7DRL.scripts;

public partial class Cell : Control
{
	[Export]
	private DragArea _draggable;
	public Slot CurrentSlot;
	
	public int Id;

	private CellDef _def;
	public string DisplayName => _def.DisplayName;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_def = CellDefs.GetCellDef(Id);
		_draggable.Texture = GD.Load<Texture2D>(_def.IconPath);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
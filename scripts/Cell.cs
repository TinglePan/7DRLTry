using Godot;

namespace Proj7DRL.scripts;

public partial class Cell : Control
{
	[Export]
	private DragArea _draggable;
	public Slot CurrentSlot;
	
	public int Id;
	public string DisplayName { get; private set; }
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var cellDef = CellDefs.GetCellDef(Id);
		DisplayName = cellDef.DisplayName;
		_draggable.Texture = GD.Load<Texture2D>(cellDef.IconPath);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
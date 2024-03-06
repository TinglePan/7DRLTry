using Godot;

namespace Proj7DRL.scripts;

public partial class ControlPanel: Control, ICellContainer
{
    [Export]
    private PackedScene _slotPrefab;
    [Export]
    private GridContainer _gridContainer;

    [Export] private int _columns;

    private Slot[,] _slots;
    
    public override void _Ready()
    {
        _gridContainer.Columns = _columns;
        _slots = new Slot[_columns, _columns];
        for (int i = 0; i < _columns; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                var slot = _slotPrefab.Instantiate<Slot>();
                slot.Coordinate = new Vector2I(j, i);
                slot.Container = this;
                _gridContainer.AddChild(slot);
                _slots[j, i] = slot;
            }
        }
    }

    public void AddItem(int itemId)
    {
        var itemPrefab = GD.Load<PackedScene>("res://cell.tscn");
        var item = itemPrefab.Instantiate<Cell>();
        item.Id = itemId;
        foreach (var slot in _slots)
        {
            if (!slot.IsOccupied)
            {
                slot.Fill(item);
                return;
            }
        }
    }
    
    public void OnAddCell(Cell cell)
    {
        
    }

    public void OnRemoveCell(Cell cell, Slot slot)
    {
        
    }
}
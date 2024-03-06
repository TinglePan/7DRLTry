using Godot;

namespace Proj7DRL.scripts;

public partial class Slot : Control, IDropCell
{
    [Export]
    private DropArea _dropArea;

    public ICellContainer Container;

    public Vector2I Coordinate;
    public Cell Content;
    
    public bool IsOccupied => Content != null;

    public void Fill(Cell cell)
    {
        Content = cell;
        cell.CurrentSlot = this;
        AddChild(cell);
        Container.OnAddCell(cell);
    }
    
    public void Clear(Cell cell)
    {
        Content = null;
        cell.CurrentSlot = null;
        RemoveChild(cell);
        Container.OnRemoveCell(cell, this);
    }

    public void DropCell(Cell cell)
    {
        var fromSlot = cell.CurrentSlot;
        Content = cell;
        Content.CurrentSlot = this;
        Container.OnAddCell(Content);
        if (fromSlot != null)
        {
            var fromContainer = fromSlot.Container;
            Content.Reparent(this, false);
            fromSlot.Content = null;
            fromContainer.OnRemoveCell(cell, fromSlot);
        }
    }
}
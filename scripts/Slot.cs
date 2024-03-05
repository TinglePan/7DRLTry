using Godot;

namespace Proj7DRL.scripts;

public partial class Slot : Control
{
    [Export]
    private Droppable _droppable;

    public ICellContainer Container;

    public Vector2I Coordinate;
    public Cell Content;
    
    public bool IsOccupied => Content != null;

    public void Fill(Cell cell)
    {
        Content = cell;
        cell.CurrentSlot = this;
        AddChild(cell);
        Container.OnAddCell(cell, this);
    }
    
    public void Clear(Cell cell)
    {
        Content = null;
        RemoveChild(cell);
        Container.OnRemoveCell(cell, this);
    }

    public void MoveContent(Slot from)
    {
        if (!from.IsOccupied) return;
        var fromContainer = from.Container;
        var movedContent = from.Content;
        Content = movedContent;
        movedContent.CurrentSlot = this;
        from.Content = null;
        movedContent.Reparent(this, false);
        fromContainer.OnRemoveCell(movedContent, from);
        Container.OnAddCell(movedContent, this);
    }
}
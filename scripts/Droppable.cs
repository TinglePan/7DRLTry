using Godot;

namespace Proj7DRL.scripts;

public partial class Droppable : TextureRect
{
    private Slot _slot;
    private bool _hovered;

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        if (_slot.IsOccupied)
        {
            return false;
        }
        
        var cell = data.As<Cell>();

        //Check if the data is our DraggableItem and if the CurrentAttachetSlot is not the same to avoid reparenting to the same parent
        if (cell != null)
        {
            _hovered = true;
            SelfModulate = Colors.Gray;
            return true;
        }

        return false;
    }

    public override void _DropData(Vector2 atPosition, Variant data)
    {
        var cell = data.As<Cell>();
        _slot.MoveContent(cell.CurrentSlot);

        SelfModulate = Colors.White;
        _hovered = false;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _slot = GetParent<Slot>();
        MouseExited += () =>
        {
            //If mouse exit this item after being hovered while dragging, then reset the color
            if (_hovered)
            {
                SelfModulate = Colors.White;
                _hovered = false;
            }
        };
    }
}
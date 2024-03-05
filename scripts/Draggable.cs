using System;
using Godot;

namespace Proj7DRL.scripts;

public partial class Draggable : TextureRect
{
	public bool IsDraggable;

	public Cell Cell;

	public override Variant _GetDragData(Vector2 atPosition)
	{
		if (!IsDraggable)
		{
			return default;
		}
		Input.SetDefaultCursorShape(Input.CursorShape.Drag);

		//Set the mouse drag by creating a copy of this
		//Do not pass the current item as is because the item will be removed automatically by Godot on drag ended
		SetDragPreview(Duplicate() as Control);

		//OPTIONAL: Since we are creating a duplicate, disable the visibility on this until dragging has completed
		Visible = false;

		return this;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Cell = GetParent<Cell>();
		// MouseDefaultCursorShape = CursorShape.Drag;
		IsDraggable = true;
	}

	// On finished drag, reset the mouse cursor shape and set it to visible again
	public override void _Notification(int what)
	{
		if (what == NotificationDragEnd)
		{
			Input.SetDefaultCursorShape();
			Visible = true;
		}
	}
}
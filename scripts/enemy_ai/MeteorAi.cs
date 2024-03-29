using System.Collections.Generic;
using Godot;

namespace Proj7DRL.scripts.enemy_ai;

public partial class MeteorAi : BaseLineMoverAi
{
	private Vector2I _dirVec;
	private State _state;
	
	enum State
	{
		Moving,
		InActive
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		_state = State.Moving;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void Proceed()
	{
		switch (_state)
		{
			case State.Moving:
				ProceedMove();
				break;
			case State.InActive:
				break;
		}
	}

	protected override void MoveBlocked(Pawn blocker)
	{
		base.MoveBlocked(blocker);
		Pawn.UseAbility(EnemyAbilities.Bump, new Dictionary<string, Variant>()
		{
			{ "target", blocker },
			{ "power", 3 },
		});
	}
}
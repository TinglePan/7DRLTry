using Godot;

namespace Proj7DRL.scripts.enemy_ai;

public interface ILineMove
{
    public Vector2 WorldPos { get; }

    public Vector2 Dir { get; }
    
    public float Step { get; }

    public void ProceedMove();
}
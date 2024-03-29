namespace Proj7DRL.scripts;

public interface ILife
{
    public int Hp { get; }

    public void TakeDamage(object src, int amount);

    public void Die();
}
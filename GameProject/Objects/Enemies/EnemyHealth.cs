namespace GameProject.Objects.Enemies;

/// <summary>Stores enemy health; only a full reset brings a defeated enemy back.</summary>
internal sealed class EnemyHealth
{
    public const int StartingHealth = 3;
    public int Health { get; private set; } = StartingHealth;
    public bool IsAlive => Health > 0;

    public bool ReceiveDamage(int amount)
    {
        if (!IsAlive || amount <= 0)
        {
            return false;
        }

        Health = Math.Max(0, Health - amount);
        return true;
    }

    public void Reset()
    {
        Health = StartingHealth;
    }
}

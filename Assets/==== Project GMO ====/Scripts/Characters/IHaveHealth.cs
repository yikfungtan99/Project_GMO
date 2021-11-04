public interface IHaveHealth
{
    public int Health { get; set; }
    public int MaxHealth { get; set; }

    public delegate void HealthChangeCallback(int health, int maxHealth);
    public event HealthChangeCallback OnHealthChanged;
}

public interface IHealth
{
    public int Health { get; set; }
    public int MaxHealth { get; set; }
}

public interface IBuffableHealth : IHealth
{
    public int BaseMaxHealth { get; set; }
    public int AdditionalMaxHealth { get; set; }
}

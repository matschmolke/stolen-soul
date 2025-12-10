public interface IDamageable
{
    CharacterData Data { get; }
    void TakeDamage(float amount);
}
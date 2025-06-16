/// <summary>
/// 피격 가능한 Entity
/// </summary>
public interface IDamagable
{
    void TakeDamage(int damage);
    void ApplyEffect(EffectType effectType);
    void Die();
}
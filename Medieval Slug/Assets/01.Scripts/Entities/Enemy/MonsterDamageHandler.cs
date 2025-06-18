using UnityEngine;

public class MonsterDamageHandler : MonoBehaviour, IDamagable
{
    [SerializeField] private Monster monster;

    private void Reset()
    {
        monster = GetComponentInChildren<Monster>();
    }

    public void TakeDamage(int damage)
    {
        monster.TakeDamage(damage);
    }

    public void ApplyEffect(EffectType effectType)
    {
        monster.ApplyEffect(effectType);
    }

    public void Die()
    {
    }
}
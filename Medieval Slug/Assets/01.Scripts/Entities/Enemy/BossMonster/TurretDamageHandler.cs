using UnityEngine;

public class TurretDamageHandler : MonoBehaviour, IDamagable
{
    [SerializeField] private BossTurret bossTurret;

    private void Reset()
    {
        bossTurret = transform.parent.parent.GetComponent<BossTurret>();
    }

    public void TakeDamage(int damage)
    {
        bossTurret.TakeDamage(damage);
    }

    public void ApplyEffect(EffectType effectType)
    {
        bossTurret.ApplyEffect(effectType);
    }

    public void Die() { }
    public void ApplyEffect(EffectType effectType)
    {

    }
}

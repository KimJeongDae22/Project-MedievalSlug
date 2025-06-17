using UnityEngine;

public class TurretDamageHandler : MonoBehaviour, IDamagable
{
    private BossTurret bossTurret;

    private void Reset()
    {
        bossTurret = transform.parent.GetComponent<BossTurret>();
    }

    public void TakeDamage(int damage)
    {
        bossTurret.TakeDamage(damage);
    }

    public void Die() { }

    public void ApplyEffect(EffectType effectType)
    {
        
    }
}

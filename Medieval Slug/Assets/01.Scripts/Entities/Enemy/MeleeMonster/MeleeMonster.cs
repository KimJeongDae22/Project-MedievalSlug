using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MeleeMonster : Monster
{
    [field : Header("Melee Monster")]
    [SerializeField] private Collider2D collider2D;

    protected override void Reset()
    {
        base.Reset();
        if (collider2D == null)
        {
            collider2D = GetComponent<Collider2D>();
            if (collider2D == null)
            {
                collider2D = gameObject.AddComponent<BoxCollider2D>();
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        DisableCollider();
    }

    public void EnableCollider()
    {
        collider2D.enabled = true;
    }

    public void DisableCollider()
    {
        collider2D.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log(other.name);
        other.TryGetComponent(out IDamagable damagable);
        if(damagable != null) 
            damagable.TakeDamage(MonsterData.Damage);
        else
            Debug.LogError($"{other.name}의 IDamagable을 찾을 수 없습니다.");
    }
}
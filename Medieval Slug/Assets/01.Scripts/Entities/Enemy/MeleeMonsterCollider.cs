using System;
using UnityEngine;

public class MeleeMonsterCollider : MonoBehaviour
{
    [SerializeField] private Monster monster;
    [SerializeField] private Collider2D collider2D;
    [SerializeField] private Vector2 colliderOffset;
    [SerializeField] private Vector2 reverseColliderOffset;

    private void Reset()
    {
        monster = transform.parent.GetComponent<Monster>();
        collider2D = GetComponentInChildren<Collider2D>();
        colliderOffset = collider2D.offset;
        reverseColliderOffset = new Vector2(-collider2D.offset.x, collider2D.offset.y);
    }

    private void Start()
    {
        DisableCollider();
    }

    public void FlipMeleeCollider(bool isFlipping)
    {
        collider2D.offset = isFlipping ? reverseColliderOffset : colliderOffset;
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
            damagable.TakeDamage(monster.MonsterData.Damage);
        else
            Debug.LogError($"{monster.name}의 IDamagable을 찾을 수 없습니다.");
    }
}

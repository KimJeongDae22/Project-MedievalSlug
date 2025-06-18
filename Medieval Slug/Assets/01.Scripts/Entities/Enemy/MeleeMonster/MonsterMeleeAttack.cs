using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMeleeAttack : MonoBehaviour
{
    [Header("Melee Monster")]
    [SerializeField] private Monster monster;
    [SerializeField] private new Collider2D collider2D;

    private void Reset()
    {
        monster = transform.parent.GetComponent<Monster>();
        collider2D = GetComponent<Collider2D>();
    }

    protected void Awake()
    {
        if (monster == null)
        {
            monster = transform.parent.GetComponent<Monster>();
        }
        if (collider2D == null)
        {
            collider2D = GetComponent<Collider2D>();
        }
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
        other.TryGetComponent(out IDamagable damagable);
        if(damagable != null) 
            damagable.TakeDamage(monster.MonsterData.Damage);
        else
            Debug.LogError($"{other.name}의 IDamagable을 찾을 수 없습니다.");
    }

    public void OnDespawn() 
    {
        monster.OnDespawn();
    }
}

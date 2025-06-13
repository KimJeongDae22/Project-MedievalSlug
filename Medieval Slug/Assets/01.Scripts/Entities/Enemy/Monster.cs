using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Monster : MonoBehaviour, IDamagable
{
    [field : SerializeField] public SpriteRenderer Sprite { get; private set; }
    [field : SerializeField] public Animator Animator { get; private set; }
    [field : SerializeField] public MonsterAnimationHash AnimationHash {get; private set;}
    [field : SerializeField] public bool HasAnimator { get; private set; } = false;
    
    [field : Header("Monster States")] 
    [field : SerializeField] public MonsterSO MonsterData { get; private set; }
    [SerializeField] private int health;

    protected virtual void Reset()
    {
        Sprite = GetComponent<SpriteRenderer>();
        
        Animator = GetComponent<Animator>();
        
        if (Animator != null) 
            HasAnimator = true;
    }

    protected virtual void Awake()
    {
        health = MonsterData.Health;
        AnimationHash.Initialize();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }
    
    public void Die()
    {
        Animator.SetTrigger("Dead");
    }

    public void DisableGameObject()
    {
        transform.parent.gameObject.SetActive(false);
        //몬스터도 오브젝트 풀링 한다면 이곳에 코드 추가
    }
}
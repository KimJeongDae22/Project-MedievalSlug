using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour, IPoolable
{
    [SerializeField] private ProjectileData projectileData;
    [SerializeField] private EffectType curEffectType;

    private Vector2 direction;
    private Rigidbody2D rigidbody;
    private float curduration;

    private Action<GameObject> returnToPool;
    [SerializeField] private EffectType curEffectType;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 shootDircetion)
    {
        this.direction = shootDircetion.normalized;
        this.curduration = 0f;

        rigidbody.velocity = direction * projectileData.ShotSpeed;
    }

    void Update()
    {
        curduration += Time.deltaTime;
        
        if (curduration >= projectileData.Range)
        {
            OnDespawn();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            if (collision.GetComponentInChildren<IDamagable>() is IDamagable target)
            {
                target.TakeDamage((int)projectileData.Damage);
                target.ApplyEffect(curEffectType);
                OnDespawn();
            }
        }
    }

    public void Initialize(Action<GameObject> returnAction)
    {
        returnToPool = returnAction;
    }

    public void OnSpawn()
    {

    }

    public void OnDespawn()
    {
        returnToPool?.Invoke(gameObject);
    }

}

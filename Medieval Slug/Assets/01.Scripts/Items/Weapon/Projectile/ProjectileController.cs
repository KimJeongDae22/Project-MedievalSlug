using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour, IPoolable
{
    [SerializeField] private ProjectileData projectileData;

    private Vector2 direction;
    private Rigidbody2D rigidbody;
    private float curduration;

    private Action<GameObject> returnToPool;

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
        if (collision.tag != "Player")
        {
        OnDespawn();
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

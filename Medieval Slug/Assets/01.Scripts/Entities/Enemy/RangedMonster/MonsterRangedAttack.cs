using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRangedAttack : MonoBehaviour
{
    [SerializeField] private Monster monster;
    [SerializeField] private Transform ShootPoint;
    [SerializeField] private Vector2 dir;

    protected void Reset()
    {
        monster = transform.parent.GetComponent<Monster>();
        ShootPoint = transform.Find("ShootPoint").transform;
    }

    protected void Awake()
    {
        if (monster == null)
        {
            monster = transform.parent.GetComponent<Monster>();
        }
        if (ShootPoint == null)
        {
            ShootPoint = transform.Find("ShootPoint").transform;
        }
        float dirX = ShootPoint.transform.position.x - transform.position.x;
        dir = new Vector2(dirX, 0).normalized;
    }

    public void RangedAttack()
    {
        float rot = Mathf.Abs(transform.rotation.eulerAngles.y);
        if (rot >= 180)
        { 
            ProjectileManager.Instance.Shoot(-dir, ShootPoint, ProjectileType.Slime);
        }
        else
        {
            ProjectileManager.Instance.Shoot(dir, ShootPoint, ProjectileType.Slime);
        }
    }

    public void OnDespawn() 
    {
        monster.OnDespawn();
    }
}

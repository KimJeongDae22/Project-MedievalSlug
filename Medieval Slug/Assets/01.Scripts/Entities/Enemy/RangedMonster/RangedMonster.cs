using UnityEngine;

public class RangedMonster : Monster
{
    [SerializeField] private Transform ShootPoint;
    [SerializeField] private Vector2 dir;

    protected override void Reset()
    {
        base.Reset();
        ShootPoint = transform.Find("ShootPoint").transform;
    }

    protected override void Awake()
    {
        base.Awake();
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
}
using UnityEngine;

public class BossTurret : MonoBehaviour
{
    public BossSO BossData;
    [SerializeField] private Transform ShootPoint;
    [SerializeField] private Vector2 dir;
    
    protected void Reset()
    {
        ShootPoint = transform.Find("ShootPoint").transform;
        float dirX = ShootPoint.transform.position.x - transform.position.x;
        dir = new Vector2(dirX, 0).normalized;
    }

    public void FireAt(Vector3 targetPosition)
    {
        
    }
}
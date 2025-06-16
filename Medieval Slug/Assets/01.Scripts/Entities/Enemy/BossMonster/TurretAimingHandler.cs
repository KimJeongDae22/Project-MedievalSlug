using System;
using UnityEngine;

public class TurretAimingHandler : MonoBehaviour
{
    [SerializeField] private BossTurretStateMachine StateMachine;
    [SerializeField] private Transform RotatingPivot;
    [SerializeField] private Transform ShootPoint; 
    [SerializeField] private Transform Target;
    [SerializeField] private float angleOffset;
    public bool isAniming = false;
    
    private void Reset()
    {
        StateMachine = transform.parent.GetComponent<BossTurretStateMachine>();
        RotatingPivot = transform.Find("RotatingPivot").transform;
        ShootPoint = transform.Find("RotatingPivot/ShootPoint").transform;
    }
    
    void Start()
    {
        Vector2 initialDirection = ShootPoint.position - RotatingPivot.position;
        angleOffset = (Mathf.Atan2(initialDirection.y, initialDirection.x) * Mathf.Rad2Deg) - 45f;
    }

    void Update()
    {
        if (Target == null)
        {
            Target = StateMachine.target != null ? StateMachine.target.transform : null;
            return;
        }

        if (isAniming)
        {
            Vector2 direction = Target.position - RotatingPivot.position;
        
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
            float finalAngle = targetAngle - angleOffset;

            RotatingPivot.rotation = Quaternion.Euler(0f, 0f, finalAngle);
        }
    }
}
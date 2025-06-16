// using UnityEngine;
//
// public class RangedMonster : Monster
// {
//     [field : Header("Ranged Monster")] 
//     [SerializeField] private ProjectileHandler projectileHandler;
//
//     protected override void Reset()
//     {
//         base.Reset();
//         projectileHandler = GetComponent<ProjectileHandler>();
//     }
//
//     public void RangedAttack()
//     {
//         projectileHandler.Shoot(transform.forward);
//     }
// }
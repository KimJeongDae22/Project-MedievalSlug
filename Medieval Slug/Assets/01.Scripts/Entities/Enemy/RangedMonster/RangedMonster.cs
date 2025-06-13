using UnityEngine;

public class RangedMonster : Monster
{
    [field : Header("Ranged Monster")]
    [field : SerializeField] public GameObject ProjectilePrefab { get; private set; }
}
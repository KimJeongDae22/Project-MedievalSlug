using System;
using UnityEngine;

[Serializable]
public class MonsterAnimationHash
{
    [SerializeField] private string idleParameterName = "Idle";
    [SerializeField] private string runParameterName = "Run";
    [SerializeField] private string attackParameterName = "Attack";
    [SerializeField] private string deadParameterName = "Dead";
    [SerializeField] private string hitParameterName = "Hit";
    
    public int IdleParameterHash {get; private set;}
    public int RunParameterHash {get; private set;}
    public int AttackParameterHash {get; private set;}
    public int DeadParameterHash {get; private set;}
    public int HitParameterHash {get; private set;}
    
    public void Initialize()
    {
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        RunParameterHash = Animator.StringToHash(runParameterName);
        AttackParameterHash = Animator.StringToHash(attackParameterName);
        DeadParameterHash = Animator.StringToHash(deadParameterName);
        HitParameterHash = Animator.StringToHash(hitParameterName);
    }
}

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
    [SerializeField] private string halfHealthParameterName = "HalfHealth";
    [SerializeField] private string appearingParameterName = "Appearing";
    [SerializeField] private string defeatParameterName = "Defeat";
    [SerializeField] private string attackTriggerParameterName = "AttackTrigger";
    
    public int IdleParameterHash {get; private set;}
    public int RunParameterHash {get; private set;}
    public int AttackParameterHash {get; private set;}
    public int DeadParameterHash {get; private set;}
    public int HitParameterHash {get; private set;}
    public int HalfHealthParameterHash {get; private set;}
    public int AppearingParameterHash {get; private set;}
    public int DefeatParameterHash {get; private set;}
    public int AttackTriggerParameterHash {get; private set;}
    
    public void Initialize()
    {
        IdleParameterHash = Animator.StringToHash(idleParameterName);
        RunParameterHash = Animator.StringToHash(runParameterName);
        AttackParameterHash = Animator.StringToHash(attackParameterName);
        DeadParameterHash = Animator.StringToHash(deadParameterName);
        HitParameterHash = Animator.StringToHash(hitParameterName);
        HalfHealthParameterHash = Animator.StringToHash(halfHealthParameterName);
        AppearingParameterHash = Animator.StringToHash(appearingParameterName);
        DefeatParameterHash = Animator.StringToHash(defeatParameterName);
        AttackTriggerParameterHash = Animator.StringToHash(attackTriggerParameterName);
    }
}

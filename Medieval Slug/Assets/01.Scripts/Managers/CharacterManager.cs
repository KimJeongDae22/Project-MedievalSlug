using Entities.Player;
using UnityEditor.U2D.Animation;
using UnityEngine;
/// <summary>
/// 전역에서 Player 각 모듈 접근용 
/// </summary>
public class CharacterManager : Singleton<CharacterManager>
{
    public PlayerController Controller { get; private set; }
    public PlayerStatHandler StatHandler { get; private set; }
    public PlayerRangedHandler PlayerRangedHandler { get; private set; }
    public PlayerItemCollector PlayerItemCollector { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        
        Controller = FindObjectOfType<PlayerController>();
        StatHandler = FindObjectOfType<PlayerStatHandler>();
        PlayerRangedHandler = FindObjectOfType<PlayerRangedHandler>();
        PlayerItemCollector = FindObjectOfType<PlayerItemCollector>();
        
        if (Controller == null) Debug.LogError("CharacterManager: No Player Controller");
        if (StatHandler == null) Debug.LogError("CharacterManager: No Stat Handler");
        if (PlayerRangedHandler == null) Debug.LogError("CharacterManager: No Player Ranged Handler");
        if (PlayerItemCollector == null) Debug.LogError("CharacterManager: No Player Item Collector");  
    }

}

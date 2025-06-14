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
        //Controller = FindObjectOfType<PlayerController>(); <= 겟컴포넌트가 더 효율적이라 바꿀게여 << 정머가
        Controller = GetComponent<PlayerController>();
        StatHandler = GetComponent<PlayerStatHandler>();
        PlayerRangedHandler = GetComponent<PlayerRangedHandler>();
        PlayerItemCollector = GetComponent<PlayerItemCollector>();
    }
}

using Entities.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        // 씬에 따른 캐릭터 위치 초기화
        switch (scene.name)
        {
            case SceneName.MAIN_SCENE:
                transform.position = new Vector3(-17, -9, 0);
                break;
            case SceneName.BOSS_SCENE:
                transform.position = new Vector3(-10, -10, 0);
                break;
        }
    }
}

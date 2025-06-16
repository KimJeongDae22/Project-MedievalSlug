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
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        base.OnSceneLoaded(scene, loadSceneMode);
        //TODO 게임 첫 시작 화면이나 특정 씬에 플레이어가 없는 경우의 수를 생각하기
    }
}

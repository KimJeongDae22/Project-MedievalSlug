using UnityEngine.SceneManagement;

public class CharacterManager : Singleton<CharacterManager>
{
    //[Header("컴포넌트 참조")]
    public PlayerController Controller { get; private set; }
    //public StatHandler StatHandler { get; private set; }
    // Animator, InventoryManager 등 추가 가능

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

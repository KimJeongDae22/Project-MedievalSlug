using UnityEngine;
using UnityEngine.SceneManagement;
public enum GameState
{
    Start, // 게임 시작 화면 
    Playing, // 게임 중
    Paused, // 일시 정지
    Boss, //보스 씬
    Ending //엔딩
}
public class GameManager : Singleton<GameManager>
{
    [field: SerializeField] public GameState State { get; private set; } = GameState.Start;
    public bool IsPlaying => State == GameState.Playing || State == GameState.Boss;
    public bool IsPaused => State == GameState.Paused;

    [Header("[Score(Coin)]")]
    [SerializeField] private int score = 0;

    [Header("Scene Names")]
    [SerializeField] string startScene = "StartScene";   // 캐릭터 선택 포함 첫 화면
    [SerializeField] string mainScene = "MainStage";    // 메인 게임
    [SerializeField] string bossScene = "BossStage";
    [SerializeField] string endingScene = "Ending";

    float sessionStartRealtime;      // 현재 세션(Playing 구간) 시작 시각
    float accumulatedTime;           // 이전 세션까지 누적된 플레이 타임
    public int Score => score;
    public float PlayTime => accumulatedTime + (IsPlaying ? Time.realtimeSinceStartup - sessionStartRealtime : 0f);


    protected override void Awake()
    {
        base.Awake();
        sessionStartRealtime = Time.realtimeSinceStartup;
    }
    /// <summary>
    /// Item.cs -> ItemType.Score에서 호출되는 메소드
    /// </summary>
    /// <param name="amount"></param>
    public void AddScore(int amount)
    {
        score = Mathf.Clamp(score + amount, 0, int.MaxValue);
        UIManager.Instance.UIUpdate_Score();
    }
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        // 특정 씬에서는 캐릭터 비활성화
        switch (scene.name)
        {
            case SceneName.INTRO_SCENE:
                CharacterManager.Instance.gameObject.SetActive(false);
                break;
            case SceneName.CHARACTER_SELECT_SCENE:
                CharacterManager.Instance.gameObject.SetActive(false);
                break;
            case SceneName.START_SCENE:
                CharacterManager.Instance.gameObject.SetActive(false);
                break;
            case SceneName.ENDING_CREDIT_SCENE:
                CharacterManager.Instance.gameObject.SetActive(false);
                break;
            default:
                CharacterManager.Instance.gameObject.SetActive(true);
                break;
        }
        // 메인 씬에 갈 때마다 캐릭터 상태 초기화
        if (scene.name == SceneName.MAIN_SCENE)
        {
            CharacterManager.Instance.StatHandler.InitializeStats();
            CharacterManager.Instance.PlayerRangedHandler.SetDefaultArrowData();
            score = 0;
            UIManager.Instance.UIUpdate_Score();
            UIManager.Instance.UIUpdate_PlayerHP();
            UIManager.Instance.UIUpdate_CurrentAmmo();
        }

    }
    /// <summary>
    /// 씬 로더 매니저 API
    /// </summary>
    /// <param name="sceneName"></param>
    void LoadSceneViaManager(string sceneName)
    {
        var loader = SceneLoadManager.Instance;
        if (loader != null) loader.LoadScene(sceneName);
        else SceneManager.LoadScene(sceneName); // 폴백
    }
    #region 씬 호출 메소드 모움
    public void LoadStart()
    {
        Time.timeScale = 1f;
        State = GameState.Start;
        LoadSceneViaManager(SceneName.START_SCENE);
    }

    public void StartMain()
    {
        ResetSessionTimer();
        score = 0;
        State = GameState.Playing;
        LoadSceneViaManager(SceneName.MAIN_SCENE);
    }

    public void StartBoss()
    {
        State = GameState.Boss;
        LoadSceneViaManager(SceneName.BOSS_SCENE);
    }

    public void LoadEnding()
    {
        Time.timeScale = 1f;
        State = GameState.Ending;
        LoadSceneViaManager(SceneName.ENDING_CREDIT_SCENE);
    }

    void ResetSessionTimer()
    {
        accumulatedTime = 0f;
        sessionStartRealtime = Time.realtimeSinceStartup;
    }

    /// <summary>
    ///  호출 시 다음 씬으로 자동 호출 
    ///  몬스터 수를 확인해서 넘어가게 하는 기능도 추가할 수도 있겠네요
    /// </summary>
    public void NextStage()
    {
        switch (State)
        {
            case GameState.Start: StartMain(); break;
            case GameState.Playing: StartBoss(); break;
            case GameState.Boss: LoadEnding(); break;
            case GameState.Ending: LoadStart(); break;
        }
    }
    #endregion

    #region 게임 일시정지/ 재개 메소드 
    public void Pause()
    {
        if (!IsPlaying) return;
        accumulatedTime += Time.realtimeSinceStartup - sessionStartRealtime;
        State = GameState.Paused;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        if (!IsPaused) return;
        sessionStartRealtime = Time.realtimeSinceStartup;
        // Boss 씬이면 Boss, 아니면 Playing
        State = SceneManager.GetActiveScene().name == bossScene ? GameState.Boss : GameState.Playing;
        Time.timeScale = 1f;
    }
    #endregion
}

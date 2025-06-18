using Unity.VisualScripting;
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

    private bool isClear;
    public bool IsClear { get {return isClear; } set { isClear = value; } }
    [Header("[Score(Coin)]")]
    [SerializeField] private int score;

    float sessionStartRealtime;      // 실질적인 플레이 시간 계산을 위해 빼야 할 시각
    float accumulatedTime;           // 누적된 플레이 타임
    public int Score => score;
    public float PlayTime => accumulatedTime - sessionStartRealtime;

    protected override void Awake()
    {
        base.Awake();
        accumulatedTime = 0;
        sessionStartRealtime = 0;
    }
    private void Update()
    {
        if (!isClear && IsPlaying && !SceneLoadManager.Instance.IsLoading)
        {
            accumulatedTime += Time.deltaTime;
        }
        if (!isClear && State == GameState.Paused || IsPlaying && SceneLoadManager.Instance.IsLoading)
        {
            sessionStartRealtime += Time.deltaTime;
        }
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
        // 특정 씬에서는 캐릭터 비활성화 및 스테 설정
        switch (scene.name)
        {
            case SceneName.INTRO_SCENE:
                CharacterManager.Instance.gameObject.SetActive(false);
                State = GameState.Start;
                break;
            case SceneName.CHARACTER_SELECT_SCENE:
                CharacterManager.Instance.gameObject.SetActive(false);
                State = GameState.Start;
                break;
            case SceneName.START_SCENE:
                CharacterManager.Instance.gameObject.SetActive(false);
                State = GameState.Start;
                break;
            case SceneName.ENDING_CREDIT_SCENE:
                CharacterManager.Instance.gameObject.SetActive(false);
                State = GameState.Ending;
                break;
            case SceneName.MAIN_SCENE:
                CharacterManager.Instance.gameObject.SetActive(true);
                State = GameState.Playing;
                break;
            case SceneName.BOSS_SCENE:
                CharacterManager.Instance.gameObject.SetActive(true);
                State = GameState.Boss;
                break;
        }
        // 메인 씬에 갈 때마다 캐릭터 상태 초기화
        if (scene.name == SceneName.MAIN_SCENE)
        {
            LoadStart();
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
        accumulatedTime = 0;
        sessionStartRealtime = 0;
        Time.timeScale = 1f;

        CharacterManager.Instance.StatHandler.InitializeStats();
        CharacterManager.Instance.PlayerRangedHandler.SetDefaultArrowData();
        score = 0;
        isClear = false;

        UIManager.Instance.UIUpdate_Score();
        UIManager.Instance.UIUpdate_PlayerHP();
        UIManager.Instance.UIUpdate_CurrentAmmo();
    }

    /// <summary>
    ///  호출 시 다음 씬으로 자동 호출 
    ///  몬스터 수를 확인해서 넘어가게 하는 기능도 추가할 수도 있겠네요
    /// </summary>
    #endregion

    #region 게임 일시정지/ 재개 메소드 
    public void Pause()
    {
        if (!IsPlaying) return;
        State = GameState.Paused;
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        if (!IsPaused) return;
        // Boss 씬이면 Boss, 아니면 Playing
        State = SceneManager.GetActiveScene().name == SceneName.BOSS_SCENE ? GameState.Boss : GameState.Playing;
        Time.timeScale = 1f;
    }
    #endregion
}

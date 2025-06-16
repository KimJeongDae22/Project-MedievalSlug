using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum FadeType
{
    In,
    Out
}
public class UIManager : Singleton<UIManager>
{
    [SerializeField] private UsuallyMessage usuallyMessage;
    [SerializeField] private Canvas canvas;
    public bool StopFunc { get; private set; }      // 기능 정지 변수. 호출은 어디서나, 값 설정은 해당 스크립트에서만
    public bool IsPaused { get; private set; }      // 일시 정지 변수, ESC 를 눌러 나오는 메뉴창 같은 상황에서 쓰임

    private Coroutine fadeCoroutine;
    private void Reset()
    {
        usuallyMessage = Util.FindChild<UsuallyMessage>(transform, "UsuallyMessage");
        canvas = Util.FindChild<Canvas>(transform, "Canvas");
    }
    protected override void Awake()
    {
        base.Awake();
        StopFunc = false;
        usuallyMessage = Util.FindChild<UsuallyMessage>(transform, "UsuallyMessage");
        canvas = Util.FindChild<Canvas>(transform, "Canvas");
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        StopAllCoroutines();
    }
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        usuallyMessage.gameObject.SetActive(false);
        // 씬이 로드될 때, 특정 씬에서는 미표시. 디폴트 값으로는 표시
        switch (scene.name)
        {
            // 첫 시작화면에는 플레이어 HUD 미표시
            case SceneName.KJD_START_SCENE:
                canvas.gameObject.SetActive(false);
                break;
            default:
                canvas.gameObject.SetActive(true);
                break;
        }
        // 타임 스케일 값 초기화
        Time.timeScale = 1f;
    }

    public void FadeInAndOut(CanvasGroup canvasGroup, float time, FadeType fadeType)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeCoroutine(canvasGroup, time, fadeType));
    }
    public IEnumerator FadeCoroutine(CanvasGroup canvasGroup, float time, FadeType fadeType)
    {
        // 만일 설정된 켄버스 그룹 컴포넌트가 없다면, 혹은 기능 정지 변수가 참이면 바로 코루틴 종료
        if (canvasGroup == null || StopFunc)
        {
            yield break;
        }
        canvasGroup.gameObject.SetActive(true);
        float percent = 0f;

        while (percent < 1f)
        {
            // 페이드 타입에 따라 러프 구간 설정
            switch (fadeType)
            {
                case FadeType.In:
                    canvasGroup.alpha = Mathf.Lerp(0f, 1f, percent);
                    break;
                case FadeType.Out:
                    canvasGroup.alpha = Mathf.Lerp(1f, 0f, percent);
                    break;
            }
            // 만약 로딩창 페이드 인/아웃일 경우 타임스케일에 영향없도록 델타 타임을 다르게 설정
            if (canvasGroup == Singleton<SceneLoadManager>.Instance.LoadingSceneCanvasGroup)
            {
                percent += Time.unscaledDeltaTime / time;
            }
            else
            {
                percent += Time.deltaTime / time;
            }
            yield return null;
        }
        // 페이드 타입에 따라 최종 투명도(알파값) 설정
        switch (fadeType)
        {
            case FadeType.In:
                canvasGroup.alpha = 1f;
                break;
            case FadeType.Out:
                canvasGroup.alpha = 0f;
                //canvasGroup.gameObject.SetActive(false);
                break;
        }

        yield break;
    }


    public void ShowUsuallyMessage(string msg, float time, Color textColor = default)
    {
        if (canvas.gameObject.activeSelf)
        {
            if (!usuallyMessage.gameObject.activeSelf)
            {
                usuallyMessage.gameObject.SetActive(true);
            }
            usuallyMessage.ShowUsuallyMessage(msg, time, textColor);
        }
    }
    public void ShowEscMenu()
    {
        // 일시 정지 기능 활성화, 로딩 중이거나 특정 씬에서는 안뜨도록 설정
        if (!Singleton<SceneLoadManager>.Instance.IsLoading)
        {
            if (canvas.gameObject.activeSelf)
            {
                if (Time.timeScale == 1f)
                {
                    Time.timeScale = 0f;
                }
                else
                {
                    Time.timeScale = 1f;
                }
                Debug.Log(Time.timeScale);
            }
        }
        // TODO Esc 누르면 나오는 메뉴 오브젝트 활성화. 오브젝트 제작 필요
    }
    private void Update()
    {
        // 기능 테스트 코드입니다.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Singleton<SceneLoadManager>.Instance.LoadScene(SceneName.KJD_START_SCENE);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Singleton<UIManager>.Instance.ShowUsuallyMessage("테스트 화면입니다." + Random.Range(1, 100).ToString(), 1f);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowEscMenu();
        }
    }
}

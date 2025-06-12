using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
    public bool IsLoading { get; set; }                 // 로딩 진행 여부 변수

    [SerializeField] private Canvas loadingSceneCanvas;
    [SerializeField] private CanvasGroup loadingSceneCanvasGroup;
    [SerializeField] private Slider progressBar;

    private string loadSceneName;                      // 로드하고자 하는 씬 이름
    [Header("로딩 화면 페이드 시간")]
    [SerializeField] private float fadeTime = 0.3f;
    private void Reset()
    {
        loadingSceneCanvas = Util.FindChild<Canvas>(transform, "Canvas");
        loadingSceneCanvasGroup = Util.FindChild<CanvasGroup>(transform, "Canvas");
        progressBar = Util.FindChild<Slider>(transform, "Slider");

        fadeTime = 0.3f;
    }
    protected override void Awake()
    {
        base.Awake();
        loadingSceneCanvas = Util.FindChild<Canvas>(transform, "Canvas");
        loadingSceneCanvasGroup = Util.FindChild<CanvasGroup>(transform, "Canvas");
        progressBar = Util.FindChild<Slider>(transform, "Slider");
    }
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == loadSceneName)
        {
            StartCoroutine(LoadCompleteEvent());
        }
    }
    private IEnumerator LoadCompleteEvent()
    {
        // 로드하고자 하는 씬이 활성화가 완료될 때까지
        yield return new WaitUntil(() => loadSceneName == SceneManager.GetActiveScene().name);
        // 원활한 씬 활성화 여부 확인을 위해 1프레임 넘기기
        yield return null;
        // 로드 완료 시 로딩 창 페이드 아웃
        Singleton<UIManager>.Instance.FadeInAndOut(loadingSceneCanvasGroup, fadeTime, FadeType.Out);

        yield return new WaitForSeconds(fadeTime);
        IsLoading = false;
    }
    public void LoadScene(string sceneName)
    {
        if (IsLoading)
        {
            return;
        }

        loadSceneName = sceneName;
        StartCoroutine(LoadSceneCoroutine());
    }
    public IEnumerator LoadSceneCoroutine()
    {
        IsLoading = true;
        progressBar.value = 0f;
        // 페이드 인으로 로딩 화면 표시
        yield return Singleton<UIManager>.Instance.FadeCoroutine(loadingSceneCanvasGroup, fadeTime, FadeType.In);
        
        // 비동기로 씬 로딩창 구현을 위한 변수 설정
        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        op.allowSceneActivation = false;

        // isDone 을 활용하여 씬 로드가 되었는지에 따라 반복문 실행
        float timer = 0.0f;
        while (!op.isDone)
        {
            timer += Time.unscaledDeltaTime;
            if (op.progress < 0.90f)
            {
                progressBar.value = Mathf.Lerp(progressBar.value, op.progress, timer);
                if (progressBar.value >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.value = Mathf.Lerp(progressBar.value, 1f, timer);
                if (progressBar.value == 1.0f)
                {
                    op.allowSceneActivation = true;
                    break;
                }
            }

            yield return null;
        }
    }
}

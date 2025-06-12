using System.Collections;
using UnityEngine;
public enum FadeType
{
    In,
    Out
}
public class UIManager : Singleton<UIManager>
{
    public bool StopFunc { get; private set; }      // 기능 정지 변수. 호출은 어디서나, 값 설정은 해당 스크립트에서만

    private Coroutine fadeCoroutine;
    private void Reset()
    {

    }
    protected override void Awake()
    {
        base.Awake();
        StopFunc = false;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        StopAllCoroutines();
        StopFunc = true;
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
            percent += Time.unscaledDeltaTime / time;
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
    private void Update()
    {
        if (!Singleton<SceneLoadManager>.Instance.IsLoading && Input.GetKeyDown(KeyCode.Space))
        {
            Singleton<SceneLoadManager>.Instance.LoadScene(SceneName.KJD_TEST_SCENE);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public enum FadeType
{
    In,
    Out
}
public class UIManager : Singleton<UIManager>
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        StopAllCoroutines();
    }
    public void FadeInAndOut(CanvasGroup canvasGroup, float time)
    {
        StartCoroutine(FadeInCoroutine(canvasGroup, time));
    }
    public IEnumerator FadeInCoroutine(CanvasGroup canvasGroup, float time)
    {
        // 만일 설정된 켄버스 그룹 컴포넌트가 없다면 바로 코루틴 종료
        if (canvasGroup == null)
        {
            yield break;
        }
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(true);
        float percent = 0f;

        while (percent < 1f)
        {
            percent += Time.unscaledDeltaTime / time;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, percent);

            yield return null;
        }
        canvasGroup.alpha = 1f;

        yield break;
    }
}

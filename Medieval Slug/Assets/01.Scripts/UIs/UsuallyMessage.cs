using System.Collections;
using TMPro;
using UnityEngine;

public class UsuallyMessage : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI messageText;

    private Coroutine coroutineUM;
    private void Reset()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        messageText = Util.FindChild<TextMeshProUGUI>(transform, "Text");
    }
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        messageText = Util.FindChild<TextMeshProUGUI>(transform, "Text");
    }
    public void ShowUsuallyMessage(string msg, float time, Color textColor)
    {
        if (coroutineUM != null)
        {
            StopCoroutine(coroutineUM);
        }
        coroutineUM = StartCoroutine(ShowUMCoroutine(msg, time, textColor));
    }
    private IEnumerator ShowUMCoroutine(string msg, float time, Color textColor)
    {
        messageText.text = msg;
        messageText.color = textColor;
        if (textColor == default)
        {
            messageText.color = Color.white;
        }
        this.gameObject.SetActive(true);
        Singleton<UIManager>.Instance.FadeInAndOut(canvasGroup, 0.3f, FadeType.In);
        yield return new WaitForSeconds(time + 0.3f);
        Singleton<UIManager>.Instance.FadeInAndOut(canvasGroup, 0.3f, FadeType.Out);
        yield return new WaitForSeconds(0.3f);
        this.gameObject.SetActive(false);
    }
}

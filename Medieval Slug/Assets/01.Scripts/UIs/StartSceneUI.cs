using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSceneUI : MonoBehaviour
{
    [SerializeField] private GameObject soundSettingBtn;
    private Canvas introCanvas;
    private RectTransform introArrow;
    private Image arrowOrbit;
    private TextMeshProUGUI titleTop;
    private TextMeshProUGUI titleBottom;

    private bool IsSkipped;

    private Coroutine myCoroutine;
    private void Start()
    {
        introCanvas = Util.FindChild<Canvas>(transform, "IntroCanvas");
        introArrow = Util.FindChild<RectTransform>(transform, "IntroArrow");
        arrowOrbit = Util.FindChild<Image>(transform, "ArrowOrbit");
        titleTop = Util.FindChild<TextMeshProUGUI>(transform, "TitleTop");
        titleBottom = Util.FindChild<TextMeshProUGUI>(transform, "TitleBottom");

        myCoroutine = StartCoroutine(ArrowMoveCoroutine());
    }
    private void Update()
    {
        if (IsSkipped)
        {
            if (Input.anyKeyDown)
            {
                EndingIntro();
            }
        }
    }
    #region 인트로 연출 메서드
    private IEnumerator ArrowMoveCoroutine()
    {
        titleTop.rectTransform.localPosition = new Vector3(0, 340, 0);
        titleBottom.rectTransform.localPosition = new Vector3(0, 340, 0);

        Scene currentScene = SceneManager.GetActiveScene();

        yield return new WaitUntil(() => currentScene.isLoaded);
        yield return null;

        IsSkipped = true;
        float time = 0;
        while (time < 1)
        {
            introArrow.localPosition = new Vector3(Util.MathfLerpEaseOut(-1100, 1100, time), 340, 0);
            arrowOrbit.rectTransform.sizeDelta = new Vector2(Util.MathfLerpEaseOut(145, 2345, time), Util.MathfLerpEaseOut(20, 5, time));


            time += Time.unscaledDeltaTime / 0.8f;

            yield return null;
        }
        myCoroutine = StartCoroutine(TitleSplitCoroutine());
    }
    private IEnumerator TitleSplitCoroutine()
    {
        arrowOrbit.color = Color.clear;

        float time = 0;
        while (time < 1)
        {
            titleTop.rectTransform.localPosition = new Vector3(Util.MathfLerpEaseOut(0, -163, time), 340, 0);
            titleBottom.rectTransform.localPosition = new Vector3(Util.MathfLerpEaseOut(0, 207, time), 340, 0);


            time += Time.unscaledDeltaTime / 1.0f;

            yield return null;
        }
        EndingIntro();
    }
    private void EndingIntro()
    {
        StopCoroutine(myCoroutine);
        introCanvas.gameObject.SetActive(false);
    }
    #endregion

    #region 버튼 클릭 메서드
    public void Btn_OnStart()
    {
        Singleton<SceneLoadManager>.Instance.LoadScene(SceneName.CHARACTER_SELECT_SCENE);
    }
    public void Btn_OnSoundSetting()
    {
        soundSettingBtn.SetActive(true);
    }
    public void Btn_OffSoundSetting()
    {
        soundSettingBtn.SetActive(false);
    }
    #endregion
}

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroScene : MonoBehaviour
{
    private Image backGround;
    private TextMeshProUGUI title;
    private TextMeshProUGUI description;
    private TextMeshProUGUI teamName;
    private IntroPlayer introPlayer;

    private string descriptionText;
    [SerializeField] private float typingTime = 0.03f;

    private bool IsSkipped;

    private Coroutine myCoroutine;
    void Start()
    {
        backGround = Util.FindChild<Image>(transform, "BG");
        title = Util.FindChild<TextMeshProUGUI>(transform, "Title");
        description = Util.FindChild<TextMeshProUGUI>(transform, "GGinGer");
        teamName = Util.FindChild<TextMeshProUGUI>(transform, "TeamName");
        introPlayer = Util.FindChild<IntroPlayer>(transform, "IntroPlayer");

        descriptionText = description.text;

        myCoroutine = StartCoroutine(TitleRotateCoroutine());
    }

    private IEnumerator TitleRotateCoroutine()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        yield return new WaitUntil(() => currentScene.isLoaded);
        yield return null;

        backGround.color = Color.white;
        title.color = Color.black;
        title.transform.localEulerAngles = new Vector3(0, 180, 0);
        description.text = string.Empty;
        teamName.text = string.Empty;

        yield return new WaitForSecondsRealtime(0.2f);

        float time = 0;
        while (time < 1)
        {
            backGround.color = Color.Lerp(Color.white, Color.black, time);
            title.color = Color.Lerp(Color.black, Color.white, time);
            title.transform.localEulerAngles = new Vector3(0, Mathf.Lerp(180, 0, time), 0);

            time += Time.unscaledDeltaTime / 2;

            yield return null;
        }
        myCoroutine = StartCoroutine(TypingCoroutine());
        IsSkipped = true;
    }
    private IEnumerator TypingCoroutine()
    {
        foreach (char c in descriptionText)
        {
            description.text += c;
            yield return new WaitForSeconds(typingTime);
        }
        myCoroutine = StartCoroutine(TeamNameFadeInCoroutine());
    }
    private IEnumerator TeamNameFadeInCoroutine()
    {
        teamName.text = "1박 3일";
        teamName.color = Color.clear;

        float time = 0;
        while (time < 1)
        {
            teamName.color = Color.Lerp(Color.clear, new Color(0, 0.6f, 1), time);


            time += Time.unscaledDeltaTime / 1.5f;

            yield return null;
        }
        myCoroutine = StartCoroutine(PlayerMoveCoroutine());
    }
    private IEnumerator PlayerMoveCoroutine()
    {
        introPlayer.AnimWalk();
        float time = 0;
        RectTransform playerRtrans = introPlayer.GetComponent<RectTransform>();
        while (time < 1)
        {
            introPlayer.AnimWalk();
            playerRtrans.localPosition = new Vector3(Mathf.Lerp(-1200, -550, time), 0, 0);


            time += Time.unscaledDeltaTime / 2;

            yield return null;
        }
        myCoroutine = StartCoroutine(PlayerAttackCoroutine());
    }
    private IEnumerator PlayerAttackCoroutine()
    {
        introPlayer.AnimAttack();

        yield return new WaitForSeconds(2);

        GameStart();
    }
    private void GameStart()
    {
        StopCoroutine(TypingCoroutine());
        SceneLoadManager.Instance.LoadScene(SceneName.KJD_START_SCENE);
    }
    private void Update()
    {
        if (IsSkipped)
        {
            if (Input.anyKeyDown)
            {
                GameStart();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ClearUI : MonoBehaviour
{
    private TextMeshProUGUI clearTitle;
    private TextMeshProUGUI score;
    private TextMeshProUGUI clearTime;
    private Button endingCreditBtn;

    private Coroutine myCoroutine;
    void Awake()
    {
        clearTitle = Util.FindChild<TextMeshProUGUI>(transform, "ClearTitle");
        score = Util.FindChild<TextMeshProUGUI>(transform, "Score");
        clearTime = Util.FindChild<TextMeshProUGUI>(transform, "ClearTime");
        endingCreditBtn = Util.FindChild<Button>(transform, "EndingCreditBtn");

        score.text = "Score : ";
        clearTime.text = "Clear time : ";
    }
    public void ClearUIEnable()
    {
        Awake();
        GameManager.Instance.IsClear = true;
        StartCoroutine(BlinkingTitleCoroutine());
        StartCoroutine(ScoreCoroutine());
    }
    private IEnumerator BlinkingTitleCoroutine()
    {
        clearTitle.color = Color.clear;

        yield return new WaitForSeconds(0.7f);

        clearTitle.color = Color.white;

        yield return new WaitForSeconds(0.7f);
        StartCoroutine(BlinkingTitleCoroutine());
    }
    private IEnumerator ScoreCoroutine()
    {
        yield return new WaitForSeconds(2f);

        score.text = $"Score : {GameManager.Instance.Score}";
        StartCoroutine(ClearTimeCoroutine());
    }
    private IEnumerator ClearTimeCoroutine()
    {
        yield return new WaitForSeconds(2f);

        clearTime.text = $"Clear time : {GameManager.Instance.PlayTime}";
        endingCreditBtn.gameObject.SetActive(true);
    }
    public void Btn_GoingEndingCredit()
    {
        SceneLoadManager.Instance.LoadScene(SceneName.ENDING_CREDIT_SCENE);
        this.gameObject.SetActive(false);
    }
}

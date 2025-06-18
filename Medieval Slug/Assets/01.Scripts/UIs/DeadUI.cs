using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeadUI : MonoBehaviour
{
    private TextMeshProUGUI gameOver;
    void Awake()
    {
        gameOver = Util.FindChild<TextMeshProUGUI>(transform, "GameOver");
    }
    public void DeadUIEnable()
    {
        Awake();
        StartCoroutine(BlinkingTitleCoroutine());
    }
    private IEnumerator BlinkingTitleCoroutine()
    {
        gameOver.color = Color.clear;

        yield return new WaitForSeconds(0.7f);

        gameOver.color = Color.white;

        yield return new WaitForSeconds(0.7f);
        StartCoroutine(BlinkingTitleCoroutine());
    }
    public void Btn_ReTry()
    {
        SceneLoadManager.Instance.LoadScene(SceneName.MAIN_SCENE);
        this.gameObject.SetActive(false);
    }
    public void Btn_StartScene()
    {
        SceneLoadManager.Instance.LoadScene(SceneName.START_SCENE);
        this.gameObject.SetActive(false);
    }
}

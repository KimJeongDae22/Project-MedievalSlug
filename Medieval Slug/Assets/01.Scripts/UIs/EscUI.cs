using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscUI : MonoBehaviour
{
    [SerializeField] private GameObject soundSettingBtn;

    public void Btn_OnEscUI()
    {
        this.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Btn_OffEscUI()
    {
        this.gameObject.SetActive(false);
        soundSettingBtn.SetActive(false);
        Time.timeScale = 1f;
    }
    public void Btn_OnSoundSetting()
    {
        soundSettingBtn.SetActive(true);
    }
    public void Btn_OffSoundSetting()
    {
        soundSettingBtn.SetActive(false);
    }
    public void Btn_OnGoingStartScene()
    {
        Btn_OffEscUI();
        Singleton<SceneLoadManager>.Instance.LoadScene(SceneName.START_SCENE);
    }
}

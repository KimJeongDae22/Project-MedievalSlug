using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneUI : MonoBehaviour
{
    [SerializeField] private GameObject soundSettingBtn;
    public void Btn_OnStart()
    {
        Singleton<SceneLoadManager>.Instance.LoadScene(SceneName.KJD_SCENE);
    }
    public void Btn_OnSoundSetting()
    {
        soundSettingBtn.SetActive(true);
    }
    public void Btn_OffSoundSetting()
    {
        soundSettingBtn.SetActive(false);
    }
}

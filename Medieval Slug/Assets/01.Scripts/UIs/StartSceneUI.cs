using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneUI : MonoBehaviour
{
    [SerializeField] private GameObject soundSettingBtn;
    public void Btn_OnStartBtn()
    {
        Singleton<SceneLoadManager>.Instance.LoadScene(SceneName.KJD_SCENE);
        Debug.Log("111");
    }
    public void Btn_OnSoundSettingBtn()
    {
        soundSettingBtn.SetActive(true);
    }
    public void Btn_OffSoundSettingBtn()
    {
        soundSettingBtn.SetActive(false);
    }
}

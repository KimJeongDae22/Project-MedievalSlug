using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    void Start()
    {
        bgmSlider.onValueChanged.AddListener(ChangeBGMVolume);
        sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);
    }
    public void ChangeBGMVolume(float value)
    {
        // TODO
        // 사운드매니저 싱글톤 할당 후 인스턴스 불러오기
        Singleton<AudioManager>.Instance.SetBGMVolume(value);

    }
    public void ChangeSFXVolume(float value)
    {
        // TODO
        Singleton<AudioManager>.Instance.SetSFXVolume(value);
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField][Range(0f, 1f)] private float soundEffectVolume; // 효과음 크기
    [SerializeField][Range(0f, 1f)] private float musicVolume; // bgm 크기
    [SerializeField][Range(0f, 1f)] private float soundEffectPitchVariance; // 음 높낮이 설정

    public List<AudioClip> BGMClip;
    public List<AudioClip> SFXClip;
    [SerializeField] private List<AudioSource> SFXPrefabs;

    [SerializeField] private int soundSourcePoolIndex = 0;

    private AudioSource musicAudioSource;
    
    protected override void Awake()
    {
        base.Awake();

        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.volume = musicVolume;
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        switch (scene.name)
        {
            case SceneName.INTRO_SCENE:
                ChangeBGM(BGMClip[0]);
                break;
            case SceneName.START_SCENE:
                ChangeBGM(BGMClip[1]);
                break;
            case SceneName.MAIN_SCENE:
                ChangeBGM(BGMClip[2]);
                break;
            case SceneName.BOSS_SCENE:
                ChangeBGM(BGMClip[4]);
                break;
            case SceneName.ENDING_CREDIT_SCENE:
                ChangeBGM(BGMClip[5]);
                break;
        }
    }

    public void ChangeBGM(AudioClip clip)
    {
        musicAudioSource.Stop();
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }

    public static void PlaySFXClip(AudioClip clip)
    {
        GameObject obj = ObjectPoolManager.Instance.GetObject(Instance.soundSourcePoolIndex, Instance.transform.position, Quaternion.Euler(0, 0, 0));
        SoundSource soundSource = obj.GetComponent<SoundSource>();
        soundSource.Play(clip, Instance.soundEffectVolume, Instance.soundEffectPitchVariance);
    }

    public void SetBGMVolume(float value)
    {
        musicVolume = value;
        musicAudioSource.volume = musicVolume;
    }

    public void SetSFXVolume(float value)
    {
        soundEffectVolume = value;
    }
}
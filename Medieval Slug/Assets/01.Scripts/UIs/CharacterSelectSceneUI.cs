using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectSceneUI : MonoBehaviour
{
    public void Btn_CharacterSelect_1()
    {
        // TODO
        // 캐릭터가 일단 하나이므로 별다른 코드 없이 씬 전환
        SceneLoadManager.Instance.LoadScene(SceneName.KJD_SCENE);
    }
    public void Btn_CharacterSelect_2()
    {
        // TODO
        // 캐릭터가 일단 하나이므로 메시지만 출력
        UIManager.Instance.ShowUsuallyMessage("현재 개발중인 캐릭터입니다.\n다음 패치를 기다려 주세요!", 1f);
    }
}

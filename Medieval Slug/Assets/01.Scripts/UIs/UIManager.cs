using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum FadeType
{
    In,
    Out
}
public class UIManager : Singleton<UIManager>
{
    private UsuallyMessage usuallyMessage;
    private ClearUI clearUI;
    private DeadUI deadUI;
    private Canvas canvas;
    private TextMeshProUGUI score;

    private TextMeshProUGUI currentAmmo;
    private Transform playerWeapon;
    private Transform vehicleWeapon;

    private EscUI escUI;

    private Transform heartParentTrans;
    private Transform hpBarParentTrans;
    private Slider hpBar;

    private GameObject overHealth;
    private TextMeshProUGUI overHealthText;
    public bool StopFunc { get; private set; }      // 기능 정지 변수. 호출은 어디서나, 값 설정은 해당 스크립트에서만
    public bool IsPaused { get; private set; }      // 일시 정지 변수, ESC 를 눌러 나오는 메뉴창 같은 상황에서 쓰임

    private Coroutine fadeCoroutine;
    private void ResetSetting()
    {
        usuallyMessage = Util.FindChild<UsuallyMessage>(transform, "UsuallyMessage");
        clearUI = Util.FindChild<ClearUI>(transform, "ClearCanvas");
        deadUI = Util.FindChild<DeadUI>(transform, "DeadCanvas");
        canvas = Util.FindChild<Canvas>(transform, "Canvas");
        score = Util.FindChild<TextMeshProUGUI>(transform, "ScoreText");
        currentAmmo = Util.FindChild<TextMeshProUGUI>(transform, "PTAmount");
        playerWeapon = Util.FindChild<RectTransform>(transform, "PlayerWeapon");
        vehicleWeapon = Util.FindChild<RectTransform>(transform, "VehicleWeapon");
        escUI = Util.FindChild<EscUI>(transform, "EscUI");
        heartParentTrans = Util.FindChild<RectTransform>(transform, "PlayerHP_Ver_Heart");
        hpBarParentTrans = Util.FindChild<RectTransform>(transform, "PlayerHP_Ver_Bar");
        hpBar = hpBarParentTrans.GetComponent<Slider>();
        overHealth = Util.FindChild<RectTransform>(transform, "OverHealth").gameObject;
        overHealthText = Util.FindChild<TextMeshProUGUI>(transform, "OverHealthText");
    }
    private void Reset()
    {
        ResetSetting();
    }
    protected override void Awake()
    {
        base.Awake();
        StopFunc = false;
        ResetSetting();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        StopAllCoroutines();
    }
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        usuallyMessage.gameObject.SetActive(false);
        // 씬이 로드될 때, 특정 씬에서는 미표시. 디폴트 값으로는 표시
        switch (scene.name)
        {
            // 첫 시작화면에는 플레이어 HUD 미표시
            case SceneName.START_SCENE:
                canvas.gameObject.SetActive(false);
                break;
            case SceneName.CHARACTER_SELECT_SCENE:
                //canvas.gameObject.SetActive(false);
                break;
            case SceneName.INTRO_SCENE:
                canvas.gameObject.SetActive(false);
                break;
            case SceneName.ENDING_CREDIT_SCENE:
                canvas.gameObject.SetActive(false);
                break;
            default:
                canvas.gameObject.SetActive(true);
                break;
        }
        // 타임 스케일 값 초기화
        Time.timeScale = 1f;
        // 플레이어 체력 UI 업데이트

    }

    public void FadeInAndOut(CanvasGroup canvasGroup, float time, FadeType fadeType)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeCoroutine(canvasGroup, time, fadeType));
    }
    public IEnumerator FadeCoroutine(CanvasGroup canvasGroup, float time, FadeType fadeType)
    {
        // 만일 설정된 켄버스 그룹 컴포넌트가 없다면, 혹은 기능 정지 변수가 참이면 바로 코루틴 종료
        if (canvasGroup == null || StopFunc)
        {
            yield break;
        }
        canvasGroup.gameObject.SetActive(true);
        float percent = 0f;

        while (percent < 1f)
        {
            // 페이드 타입에 따라 러프 구간 설정
            switch (fadeType)
            {
                case FadeType.In:
                    canvasGroup.alpha = Mathf.Lerp(0f, 1f, percent);
                    break;
                case FadeType.Out:
                    canvasGroup.alpha = Mathf.Lerp(1f, 0f, percent);
                    break;
            }
            // 만약 로딩창 페이드 인/아웃일 경우 타임스케일에 영향없도록 델타 타임을 다르게 설정
            if (canvasGroup == Singleton<SceneLoadManager>.Instance.LoadingSceneCanvasGroup)
            {
                percent += Time.unscaledDeltaTime / time;
            }
            else
            {
                percent += Time.deltaTime / time;
            }
            yield return null;
        }
        // 페이드 타입에 따라 최종 투명도(알파값) 설정
        switch (fadeType)
        {
            case FadeType.In:
                canvasGroup.alpha = 1f;
                break;
            case FadeType.Out:
                canvasGroup.alpha = 0f;
                //canvasGroup.gameObject.SetActive(false);
                break;
        }

        yield break;
    }
    public void ShowUsuallyMessage(string msg, float time, Color textColor = default)
    {

        if (!usuallyMessage.gameObject.activeSelf)
        {
            usuallyMessage.gameObject.SetActive(true);
        }
        usuallyMessage.ShowUsuallyMessage(msg, time, textColor);

    }
    public void ShowEscUI()
    {
        // 일시 정지 기능 활성화, 로딩 중이거나 특정 씬에서는 안뜨도록 설정
        if (!SceneLoadManager.Instance.IsLoading)
        {
            if (canvas.gameObject.activeSelf && !clearUI.gameObject.activeSelf && !CharacterManager.Instance.StatHandler.IsDied)
            {
                if (Time.timeScale == 1f)
                {
                    escUI.gameObject.SetActive(true);
                    escUI.Btn_OnEscUI();
                }
                else
                {
                    escUI.Btn_OffEscUI();
                }
                // Debug.Log(Time.timeScale);
            }
        }
        // TODO Esc 누르면 나오는 메뉴 오브젝트 활성화. 오브젝트 제작 필요
    }
    public void ShowClearUI()
    {
        clearUI.gameObject.SetActive(true);
        clearUI.ClearUIEnable();
    }
    public void ShowDeadUI()
    {
        deadUI.gameObject.SetActive(true);
        deadUI.DeadUIEnable();
    }
    public void UIUpdate_CurrentAmmo()
    {
        int crAmmoInt = Singleton<CharacterManager>.Instance.PlayerRangedHandler.GetCurrentAmmo();

        currentAmmo.text = crAmmoInt > 0 ? crAmmoInt.ToString() : "--";
    }
    public void UIUpdate_PlayerHP()
    {
        int heartAmount = (int)CharacterManager.Instance.StatHandler.GetStat(StatType.Health);
        if (heartAmount < 0)
        {
            return;
        }
        if (heartAmount <= 5) // 최대 하트 UI 표시량
        {
            overHealth.SetActive(false);

            // 플레이어 체력에 해당하는 만큼 하트 UI 표시
            for (int i = 0; i < heartAmount; i++)
            {
                heartParentTrans.GetChild(i).gameObject.SetActive(true);
            }
            // 플레이어 체력이 넘어가는 하트 표시 비활성화
            for (int i = heartAmount; i < heartParentTrans.childCount; i++)
            {
                heartParentTrans.GetChild(i).gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < heartParentTrans.childCount; i++)
            {
                heartParentTrans.GetChild(i).gameObject.SetActive(true);
            }
            overHealth.SetActive(true);
            overHealthText.text = $"+{heartAmount - 5}";
        }
    }
    public void UIUpdate_Score()
    {
        score.text = GameManager.Instance.Score.ToString();
    }
    public void UIUpdate_TankUI()
    {
        if (CharacterManager.Instance.Controller.IsMounted)
        {
            heartParentTrans.gameObject.SetActive(false);
            hpBarParentTrans.gameObject.SetActive(true);
            vehicleWeapon.gameObject.SetActive(true);
            playerWeapon.gameObject.SetActive(false);

            VehicleController tank = CharacterManager.Instance.Controller.CurrentVehicle;

            hpBar.value = tank.VehicleHP() / tank.MaxHp;
        }
        else
        {
            heartParentTrans.gameObject.SetActive(true);
            hpBarParentTrans.gameObject.SetActive(false);
            vehicleWeapon.gameObject.SetActive(false);
            playerWeapon.gameObject.SetActive(true);
        }
    }
    private void Update()
    {
        // 기능 테스트 코드입니다.
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Singleton<SceneLoadManager>.Instance.LoadScene(SceneName.START_SCENE);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Singleton<UIManager>.Instance.ShowUsuallyMessage("테스트 화면입니다." + Random.Range(1, 100).ToString(), 1f);
            CharacterManager.Instance.StatHandler.TakeDamage(1);
            UIUpdate_PlayerHP();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Singleton<UIManager>.Instance.ShowUsuallyMessage("<color=pink> 체력을 회복합니다.</color>", 1f);
            CharacterManager.Instance.StatHandler.TakeDamage(-1);
            UIUpdate_PlayerHP();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            ShowClearUI();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowEscUI();
        }
    }
}

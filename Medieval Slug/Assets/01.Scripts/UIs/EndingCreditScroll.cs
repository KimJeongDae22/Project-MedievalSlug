using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndingCreditScroll : MonoBehaviour
{
    private TextMeshProUGUI scrollText;
    private Button startSceneBtn;
    public float ScrollSpeed = 30;
    private bool isSkipped;
    void Start()
    {
        scrollText = Util.FindChild<TextMeshProUGUI>(transform, "Text");
        startSceneBtn = Util.FindChild<Button>(transform, "Btn");
        Invoke("Skip", 2f);
    }
    void Skip()
    {
        isSkipped = true;
    }
    public void Btn_StartScene()
    {
        SceneLoadManager.Instance.LoadScene(SceneName.START_SCENE);
    }
    void Update()
    {
        float speed = ScrollSpeed;

        if (Input.anyKeyDown && isSkipped)
        {
            startSceneBtn.gameObject.SetActive(true);
        }
        if (Input.anyKey)
        {
            speed = ScrollSpeed * 2;
        }
        else
        {
            speed = ScrollSpeed;
        }
        if (scrollText.rectTransform.localPosition.y < 3100)
        {
            scrollText.rectTransform.localPosition += new Vector3(0, speed, 0);
        }
    }
}

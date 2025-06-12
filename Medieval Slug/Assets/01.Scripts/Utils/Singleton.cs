using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<T>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    instance = singletonObject.AddComponent<T>();
                }
            }
            return instance;
        }
    }
    
    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
            
        else
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        // 싱글톤을 상속받는 클래스가 씬이 로딩 될 때 실행되는 함수를 이벤트에 추가
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    protected virtual void OnDestroy()
    {
        // 이미 싱글톤을 상속받은 해당 클래스가 존재하면 파괴되므로 씬로딩 이벤트에 함수 제외
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {

    }
}
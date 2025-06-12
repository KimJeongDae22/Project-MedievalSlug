using UnityEditor.U2D.Animation;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    //[Header("컴포넌트 참조")]
    public PlayerController Controller { get; private set; }
    //public StatHandler StatHandler { get; private set; }
    // Animator, InventoryManager 등 추가 가능

    protected override void Awake()
    {
      base.Awake();
        Controller = FindObjectOfType<PlayerController>();
    }
}

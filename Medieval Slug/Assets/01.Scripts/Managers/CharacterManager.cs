using Entities.Player;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class CharacterManager : Singleton<CharacterManager>
{
    public PlayerController Controller { get; private set; }
    public PlayerStatHandler StatHandler { get; private set; }
    public PlayerEquip PlayerEquip { get; private set; }


    protected override void Awake()
    {
      base.Awake();
        Controller = FindObjectOfType<PlayerController>();
    }
}

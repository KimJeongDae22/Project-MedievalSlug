using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroPlayer : MonoBehaviour
{
    private Image image;
    private Sprite lastSprite;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
        lastSprite = image.sprite;
    }
    void LateUpdate()
    {
        if (image.sprite != lastSprite)
        {
            lastSprite = image.sprite;
            image.SetNativeSize();
        }
    }
    public void AnimWalk()
    {
        animator.SetBool("IsAttack", false);
        animator.SetBool("IsWalk", true);
    }
    public void AnimAttack()
    {
        animator.SetBool("IsAttack", true);
        animator.SetBool("IsWalk", false);
    }
    public void AnimIdle()
    {
        animator.SetBool("IsAttack", false);
        animator.SetBool("IsWalk", false);
    }
}

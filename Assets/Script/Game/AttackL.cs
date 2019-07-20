using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackL : MonoBehaviour
{
    // 以下メンバ変数定義.
    Animator animator_l = null;
    CapsuleCollider collider_l = null;
    AudioSource clip = null;


    void Start()
    {
        animator_l = GetComponent<Animator>();
        collider_l = GetComponent<CapsuleCollider>();
        clip = GetComponent<AudioSource>();
    }

    void Update()
    {
        //左クリックで攻撃のアニメーションと効果音を出す
        if (Input.GetMouseButtonDown(0))
        {
            if (!animator_l.GetCurrentAnimatorStateInfo(0).IsName("Attack_l"))
            {
                animator_l.SetTrigger("LTrigger");
            }
            AnimatorStateInfo info = animator_l.GetCurrentAnimatorStateInfo(0);
            
            animator_l.Play(info.fullPathHash, 0, 0.0f);
            clip.Play();
        }

    }
    
    //アニメーションの開始と同時にColliderを出す
    void AttackStartL()
    {
        collider_l.enabled = true;
    }

    //アニメーションの終了と同時にColliderを消す、アニメーションを攻撃から待機へ戻す
    void AttackEndL()
    {
        collider_l.enabled = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackR : MonoBehaviour
{
    // 以下メンバ変数定義.
    Animator animator_r = null;
    CapsuleCollider collider_r = null;
    AudioSource clip = null;
    

    void Start()
    {
        animator_r = GetComponent<Animator>();
        collider_r = GetComponent<CapsuleCollider>();
        clip = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        //右クリックで攻撃のアニメーションと効果音を出す
        if (Input.GetMouseButtonDown(1))
        {
            if (!animator_r.GetCurrentAnimatorStateInfo(0).IsName("Attack_r"))
            {
                animator_r.SetTrigger("RTrigger");
            }
            AnimatorStateInfo info = animator_r.GetCurrentAnimatorStateInfo(0);

            animator_r.Play(info.fullPathHash, 0, 0.0f);
            clip.Play();
        }

    }

    //アニメーションの開始と同時にColliderを出す
    void AttackStartR()
    {
        collider_r.enabled = true;
    }

    //アニメーションの終了と同時にColliderを消す、アニメーションを攻撃から待機へ戻す
    void AttackEndR()
    {
        collider_r.enabled = false;
    }
    
}

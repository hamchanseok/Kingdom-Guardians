using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private BoxCollider swordcol;
    private bool isAttack = false; // 공격 가능 여부
    public int atk = 10;
    public ParticleSystem slash;

    private AudioSource audiosource;

    public void Start()
    {
        swordcol = GetComponent<BoxCollider>();
        audiosource = GetComponent<AudioSource>();
    }

    public void OnAttack()
    {
        CancelInvoke("EndAttack"); // 혹시 모를 중복 호출 방지

        isAttack = true;
        swordcol.enabled = true;
        slash.Play();
        audiosource.PlayOneShot(audiosource.clip);
        

        Invoke("EndAttack", 0.5f);
    }
    private void EndAttack()
    {
        isAttack = false; // 공격 불가능
        swordcol.enabled = false; // 충돌 비활성화
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 적인지 확인
        if (other.CompareTag("Enemy"))
        {
            // EnemyManager를 시도해서 가져오기
            EnemyManager enemyManager = other.GetComponent<EnemyManager>();

            if (enemyManager != null && isAttack)
            {
                // EnemyManager가 있으면 피해를 입힘
                enemyManager.TakeDamage(atk);
                isAttack = false; // 공격 불가능
            }
            else
            {
                // EnemyManager가 없으면 BossStats를 시도해서 가져오기
                BossStats bossStats = other.GetComponentInParent<BossStats>();

                if (bossStats != null && isAttack)
                {
                    // BossStats가 있으면 피해를 입힘
                    bossStats.TakeDamage(atk);
                    isAttack = false; // 공격 불가능
                    swordcol.enabled = false;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage = 10;
    private float lifeTime = 0.0f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime > 5.0f)
        {
            Destroy(gameObject, 0.2f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 오브젝트가 적인지 확인
        if (other.CompareTag("Enemy"))
        {
            // EnemyManager를 시도해서 가져오기
            EnemyManager enemyManager = other.GetComponent<EnemyManager>();

            if (enemyManager != null)
            {
                // EnemyManager가 있으면 피해를 입힘
                enemyManager.TakeDamage(damage);
                
            }
            else
            {
                // EnemyManager가 없으면 BossStats를 시도해서 가져오기
                BossStats bossStats = other.GetComponentInParent<BossStats>();

                if (bossStats != null)
                {
                    // BossStats가 있으면 피해를 입힘
                    bossStats.TakeDamage(damage);
                    
                }
            }
        }

        Destroy(gameObject);
    }
   
}

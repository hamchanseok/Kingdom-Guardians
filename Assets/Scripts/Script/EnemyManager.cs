using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyManager : MonoBehaviour
{
    public enum State
    {
        Idle,
        Chase,
        Attack,
        Dead
    }

    public ParticleSystem Hit;
    public State currentState = State.Idle;
    private Animator animator;
    private EnemyStats enemyStats;
    private PlayerStats playerStats;
    private AudioSource audioSource;

    private float attackTimer = 0.0f;
    private float attackDelay = 2.0f;

    private float chaseDistance = 1000.0f;
    private float attackDistance = 2.5f;
    private float reChaseDistance = 3.0f;

    private bool isDead = false;
    private Transform player; // Player를 인스펙터에서 설정하기 위한 변수

    NavMeshAgent nav;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyStats = GetComponent<EnemyStats>();
        enemyStats.deadEvent.AddListener(CallDeadEvent);
        nav = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = player.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
    }

    private void UpdateState()
    {
        switch (currentState)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Chase:
                ChaseState();
                break;
            case State.Attack:
                AttackState();
                break;
            case State.Dead:
                DeadState();
                break;
        }
    }

    private void IdleState()
    {
        if (GetDistanceFromPlayer() < chaseDistance)
        {
            ChangeState(State.Chase);
        }
    }

    private void ChaseState()
    {
        nav.SetDestination(player.position);

        if (GetDistanceFromPlayer() < attackDistance)
        {
            ChangeState(State.Attack);
           
        }
    }

    private void AttackState()
    {
        if (GetDistanceFromPlayer() > reChaseDistance)
        {
            attackTimer = 0;
            ChangeState(State.Chase);
        }
        else
        {
            if (attackTimer > attackDelay)
            {
                transform.LookAt(player.position);
                AttackCalculate();
                attackTimer = 0f;
            }
            attackTimer += Time.deltaTime;
        }
    }

    private void DeadState()
    {
        if(!isDead)
        {
            animator.SetTrigger("Dead"); // 사망 애니메이션 재생
            GetComponent<Collider>().enabled = false; // 콜라이더 비활성화
            Destroy(gameObject, 2f); // 일정 시간 후 오브젝트 파괴
            nav.isStopped = true;

            isDead = true;
        }        
    }

    private void ChangeState(State newState)
    {
        currentState = newState;
    }

    private float GetDistanceFromPlayer()
    {
        return Vector3.Distance(transform.position, player.position);
    }

    private void AttackCalculate()
    {
        int damage = enemyStats.enemyDamage;
        playerStats.SetEnemyAttack(damage);
    }

    private void CallDeadEvent()
    {
        ChangeState(State.Dead);
    }

    public void TakeDamage(int damage)
    {
        enemyStats.statCurHP -= damage;
        Hit.Play();
        audioSource.PlayOneShot(audioSource.clip);

        if (enemyStats.statCurHP <= 0)
        {
            CallDeadEvent();
        }
    }
}
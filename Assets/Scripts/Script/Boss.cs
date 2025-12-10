using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public enum BossState { Walk, Bite, Breath, Claw, Die }
    public BossState currentState = BossState.Walk;

    public float attackDistance = 5f;
    public float clawAttackDistance = 10f;
    public float attackRange = 2f;

    private Transform player;
    private NavMeshAgent agent;
    private Animator animator;

    public Transform mouthPosition;  // 드래곤 입 위치 (빈 오브젝트)
    public Transform leftHandPosition;  // 왼손 위치
    public Transform rightHandPosition;  // 오른손 위치

    public GameObject firePrefab;  // 복제할 불꽃 프리팹
    public float fireInterval = 0.5f;  // 불꽃 발사 간격

    private bool isAttacking = false;
    private bool isBreathingFire = false;
    private float fireTimer = 0f;

    private AudioSource audioSource;
    public AudioClip[] attackSounds;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        SetState(BossState.Walk);
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (isBreathingFire)
        {
            HandleBreathFire();
        }

        if (!isAttacking)
        {
            if (distanceToPlayer <= clawAttackDistance)
            {
                ChooseAttackBasedOnDistance(distanceToPlayer);
                isAttacking = true;
            }
            else
            {
                SetState(BossState.Walk);
            }
        }
    }

    public void SetState(BossState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case BossState.Walk:
                agent.isStopped = false;
                agent.SetDestination(player.position);
                animator.SetInteger("Dragon", 0);  // Walk 애니메이션 재생
                break;

            case BossState.Claw:
                agent.isStopped = true;
                animator.SetInteger("Dragon", 1);  // Claw 애니메이션 재생
                StartCoroutine(PlaySoundWithDelay(1, 0));
                transform.LookAt(player.position);
                break;

            case BossState.Bite:
                agent.isStopped = true;

                animator.SetInteger("Dragon", 2);  // Bite 애니메이션 재생
                transform.LookAt(player.position);
                StartCoroutine(PlaySoundWithDelay(2, 0));
                break;

            case BossState.Breath:
                agent.isStopped = true;

                animator.SetInteger("Dragon", 3);  // Breath 애니메이션 재생
                StartCoroutine(PlaySoundWithDelay(3, 0));
                transform.LookAt(player.position);
                break;

            case BossState.Die:
                agent.isStopped = true;
                animator.SetTrigger("Die");
                StartCoroutine(PlaySoundWithDelay(4, 0));
                break;
                
        }
    }

    private void ChooseAttackBasedOnDistance(float distanceToPlayer)
    {
        if (distanceToPlayer > attackDistance && distanceToPlayer <= clawAttackDistance)
        {
            int randomAttack = Random.Range(0, 2); // Bite 또는 Breath 선택
            switch (randomAttack)
            {
                case 0:
                    SetState(BossState.Claw);
                    break;

                case 1:
                    SetState(BossState.Breath);
                    break;
            }
        }
        else if (distanceToPlayer <= attackDistance)
        {
            SetState(BossState.Bite);
        }
    }
    public void EndAttack()
    {
        isAttacking = false;
    }

    // 불꽃을 연속으로 생성 및 발사하는 메서드
    public void StartBreathFire()
    {
        isBreathingFire = true;
        fireTimer = 0f;
    }

    private void HandleBreathFire()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireInterval)
        {
            fireTimer = 0f;
            GameObject fireInstance = Instantiate(firePrefab, mouthPosition.position, mouthPosition.rotation);
            Rigidbody fireRb = fireInstance.GetComponent<Rigidbody>();

            if (fireRb != null)
            {
                fireRb.velocity = mouthPosition.forward * 10f;  // 원하는 속도로 발사

                Destroy(fireInstance, 1.2f);
            }
        }
    }
    public void StopBreathFire()
    {
        isBreathingFire = false;
    }

    public void CheckBiteAttack()
    {

        if (BiteAttackRange())
        {
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            playerStats.SetEnemyAttack(10);
            Debug.Log("물기로 데미지 입음");
        }
    }
    public void CheckClawAttack()
    {

        if (HandAttackRange())
        {
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            playerStats.SetEnemyAttack(15);
            Debug.Log("할퀴기로 데미지 입음");
        }
    }
    private bool BiteAttackRange()
    {
        float distanceToPlayer = Vector3.Distance(mouthPosition.position, player.position);
        return distanceToPlayer <= attackRange;
    }

    private bool HandAttackRange()
    {
        float distanceToLeftHand = Vector3.Distance(leftHandPosition.position, player.position);
        float distanceToRightHand = Vector3.Distance(rightHandPosition.position, player.position);

        return distanceToLeftHand <= attackRange || distanceToRightHand <= attackRange;
    }

    private IEnumerator PlaySoundWithDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (audioSource != null && attackSounds != null && index < attackSounds.Length && attackSounds[index] != null)
        {
            audioSource.PlayOneShot(attackSounds[index]);
        }
    }


    private void OnDrawGizmos()
    {
        // 손 위치에 대한 공격 범위 기즈모
        if (leftHandPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(leftHandPosition.position, attackRange);
        }

        if (rightHandPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(rightHandPosition.position, attackRange);
        }

        // 입 위치에 대한 공격 범위 기즈모
        if (mouthPosition != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(mouthPosition.position, attackRange);
        }
    }

}

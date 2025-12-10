using MNF;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using static Boss;

public class PlayerManager : MonoBehaviour
{
    
    public float moveSpeed = 1.0f; // 이동 속도
    public float rotateSpeed = 150.0f; // 회전속도
    public PlayerState currentState = PlayerState.Idle; //기본 상태값 Idle

    private Coroutine rollCoroutine;
    private PlayerAni myAni;
    public PlayerStats playerStats;
    public ParticleSystem spearEffect;

    
    public enum WeaponType { None, Sword, Bow, Spear }
    public WeaponType currentWeapon = WeaponType.Sword;


    public GameObject sword;  // Sword 오브젝트
    public GameObject bow;    // Bow 오브젝트
    public GameObject spear;  // Spear 오브젝트


    private Bow bowScript;
    private Sword swordScript;
    private Spear spearScript;
    private Potion potion;

    NetVector3 prevTransform0 = new NetVector3(0, 0, 0);
    NetVector3 prevTransform1 = new NetVector3(0, 0, 0);
    public TextMesh userID;

    //private AudioSource audioSource;
    //public AudioClip[] attackSounds;

    // Start is called before the first frame update
    void Start()
    {
        myAni = GetComponent<PlayerAni>();
        playerStats = GetComponent<PlayerStats>();

        swordScript = sword.GetComponent<Sword>();
        spearScript = spear.GetComponent<Spear>();
        bowScript = bow.GetComponent<Bow>();

        //audioSource = GetComponent<AudioSource>();

        UserSession userSession = NetGameManager.instance.GetRoomUserSession(
          NetGameManager.instance.m_userHandle.m_szUserID);
        if (userSession != null)
        {
            Init(userSession);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void CurrentEnemyDead()
    {
        ChangeState(PlayerState.Idle);

    }

    public void ChangeToPlayerDead() //HP 0 되면 플레이어 사망처리
    {
        if (playerStats.statCurHP <= 0)
        {
            ChangeState(PlayerState.Die);
            UIManager.s.GameOver();
            UIManager.s.CursorOn();
            CameraController.CameraFreeze = true;
            this.enabled = false;
        }

    }

    void ChangeState(PlayerState newState)
    {
        if (currentState == newState)
            return;

        myAni.ChangeAni(newState);
        currentState = newState;
    }

    void ChangeIdle() //Idle로 변경하는 함수
    {
        myAni.ChangeAni(PlayerState.Idle);
        currentState = PlayerState.Idle;

    }

    // Update is called once per frame
    void Update()
    {
        ChangeToPlayerDead();
        HandleMovement();
        Attack();
        Roll();
        

    }


    void HandleMovement() // 이동 로직
    {
        float horizontalInput = Input.GetAxis("Horizontal1");
        float verticalInput = Input.GetAxis("Vertical1");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (moveDirection != Vector3.zero && currentState != PlayerState.SwordAttack && currentState
            != PlayerState.SpearAttack && currentState != PlayerState.BowAttack && currentState != PlayerState.roll)
        {
            // 카메라가 바라보는 방향을 기준으로 이동 방향을 조정합니다.
            moveDirection = Camera.main.transform.TransformDirection(moveDirection);
            moveDirection.y = 0; // Y축 방향 제거
            moveDirection.Normalize(); // 정규화

            // 플레이어를 입력 방향으로 회전시킵니다.
            transform.rotation = Quaternion.LookRotation(moveDirection);

            // 플레이어를 입력 방향으로 이동시킵니다.
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

            // Walk 상태로 변경합니다.
            ChangeState(PlayerState.Walk);

        }
        else if (moveDirection == Vector3.zero && currentState == PlayerState.Walk)
        {
            // 입력이 없는 경우 Idle 상태로 변경합니다.
            ChangeState(PlayerState.Idle);


            prevTransform0 = new NetVector3(transform.position);
            prevTransform1 = new NetVector3(transform.rotation.eulerAngles);
            UserSession userSession = NetGameManager.instance.GetRoomUserSession(
              NetGameManager.instance.m_userHandle.m_szUserID);

            if (prevTransform0.Equals(userSession.m_userTransform[0]) && prevTransform1.Equals(userSession.m_userTransform[1]))
                return;

            userSession.m_userTransform[0] = prevTransform0;
            userSession.m_userTransform[1] = prevTransform1;

            NetManager.instance.Send_ROOM_USER_MOVE_DIRECT(userSession);
        }
    }


    // 플레이어가 마우스 좌클릭으로 공격할 때 호출
    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            switch (currentWeapon)
            {
                case WeaponType.Sword:
                    ChangeState(PlayerState.SwordAttack);//SwordAttack 애니로 바꿈
                    break;
                case WeaponType.Bow:
                    ChangeState(PlayerState.BowAttack);//BowAttack 애니로 바꿈
                    break;
                case WeaponType.Spear:
                    ChangeState(PlayerState.SpearAttack);//SpearAttack 애니로 바꿈
                    break;
            }
        }
    }


    public void ActiveAttack() //애니메이션 이벤트를 통해 호출 받아 공격할 수 있는 상태 활성화
    {
        switch (currentState)
        {
            case PlayerState.SwordAttack:
                {
                    swordScript.OnAttack();
                    Debug.Log("호출됨");
                    break;
                }

            case PlayerState.SpearAttack:
                {
                    spearScript.OnAttack();
                    Debug.Log("호출됨2");
                    break;
                }
        }
    }

    public void ActiveEffect()
    {
        spearEffect.Play();
    }

    // 플레이어가 공격할 때 호출되는 메서드
    public void AttackEnemy(EnemyManager enemyManager)
    {
        // 적의 체력을 감소시키는 등의 공격 동작을 수행
        int damage = playerStats.enemyDamage;
        enemyManager.TakeDamage(damage);
    }     
    public void Roll() // 구르기
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentState != PlayerState.roll)
        {
            ChangeState(PlayerState.roll);

            if (rollCoroutine != null)
            {
                StopCoroutine(rollCoroutine);
            }

            rollCoroutine = StartCoroutine(RollForward());
     
        }
    }

    private IEnumerator RollForward()
    {
        float rollSpeed = 5f; // 구르기 속도
        float rollDuration = 1f; // 구르기 지속 시간
        float timer = 0;

        while (timer < rollDuration)
        {
            // 플레이어가 전방으로 이동
            transform.Translate(Vector3.forward * rollSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        rollCoroutine = null;


    }

    public void Shoot()
    {
        bowScript.ShootArrow();
    }

    public void Heal(int amount)
    {
        if (playerStats != null)
        {
            playerStats.Heal(amount); // PlayerStats의 Heal 메서드를 호출하여 체력 회복
        }
    }

    public void Init(UserSession user)
    {
        Debug.Log("Initializing player with userID: " + user.m_szUserID);
        gameObject.name = user.m_szUserID;
        userID.text = user.m_szUserID;
    }





    /*
    private void PlayerSound()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                
                break;

            case PlayerState.Walk:
                
                break;

            case PlayerState.SwordAttack:
                StartCoroutine(PlaySoundWithDelay(2, 0));
                break;

            case PlayerState.SpearAttack:
                
                break;

            case PlayerState.BowAttack:
                
                break;

            case PlayerState.roll:
                break;

        }
    }
    private IEnumerator PlaySoundWithDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (audioSource != null && attackSounds != null && index < attackSounds.Length && attackSounds[index] != null)
        {
            audioSource.PlayOneShot(attackSounds[index]);
        }
    }
    */


}
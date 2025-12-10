using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Potion : MonoBehaviour
{
    public int healAmount = 20;
    public float coolDownTime = 5f;
    private bool isOnCoolDown = false;
    public int potionCount = 0; // 포션 개수 설정
    public ParticleSystem healingEffect;

    private PlayerManager playerManager;

    public Image cooldownImage; // 쿨타임 이미지 설정

    void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        if (cooldownImage != null)
        {
            cooldownImage.gameObject.SetActive(false); // 시작 시 이미지 비활성화
        }
    }

    public void UsePotion()
    {
        if (isOnCoolDown)
        {
            Debug.Log("Potion is on cooldown.");
            return;
        }

        if (potionCount <= 0)
        {
            Debug.Log("No potions left.");
            return;
        }

        if (playerManager != null)
        {
            playerManager.Heal(healAmount); // PlayerManager의 Heal 메서드 호출
            healingEffect.Play();
            potionCount--; // 포션 개수 감소
            StartCoroutine(StartCooldown());

            // 포션 개수가 0이 되면 시각적으로 슬롯을 비활성화하도록 SlotManager에 알림을 줄 수도 있음
            if (potionCount <= 0)
            {
                Debug.Log("All potions used up.");
            }
        }
    }

    private IEnumerator StartCooldown()
    {
        isOnCoolDown = true;

        cooldownImage.gameObject.SetActive(true);
        cooldownImage.fillAmount = 1f;

        float elapsedTime = 0f;

        while (elapsedTime < coolDownTime)
        {
            elapsedTime += Time.deltaTime;
            if (cooldownImage != null)
            {
                cooldownImage.fillAmount = 1f - (elapsedTime / coolDownTime);
            }
            yield return null;
        }

        cooldownImage.fillAmount = 0f;
        cooldownImage.gameObject.SetActive(false); // 쿨타임 종료 시 이미지 비활성화

        isOnCoolDown = false;
    }
}

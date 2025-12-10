using UnityEngine;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour
{
    public GameObject object1; // 칼 오브젝트
    public GameObject object2; // 창 오브젝트
    public GameObject object3; // 석궁 오브젝트
    public Potion potionScript;

    public GameObject slot1; // 칼 슬롯
    public GameObject slot2; // 창 슬롯
    public GameObject slot3; // 석궁 슬롯
    public GameObject slot4; // 포션 슬롯
    public Text potionCountText;


    public PlayerManager playerManager; //플레이어 매니저 참조

    private Vector3 originalScale; // 슬롯의 원래 크기
    private float enlargedScaleFactor = 1.2f; // 크기 조정 배율
    private Color transparentColor = new Color(1, 1, 1, 0.5f); // 투명도 128 (0.5 알파)
    private Color opaqueColor = new Color(1, 1, 1, 1); // 불투명

    private bool isKey2Locked = true; // 2번 키 잠금 상태
    private bool isKey3Locked = true; // 3번 키 잠금 상태    

    void Start()
    {
        originalScale = slot1.transform.localScale; // 슬롯의 원래 크기 저장
        // 게임 시작 시 칼 슬롯만 활성화
        ActivateObject(1);
        slot2.SetActive(false); // 창 슬롯 비활성화
        slot3.SetActive(false); // 석궁 슬롯 비활성화
        UpdatePotionCount();
    }

    void Update()
    {
        // 키 입력에 따라 오브젝트 활성화/비활성화
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerManager.currentWeapon = PlayerManager.WeaponType.Sword;
            ActivateObject(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && !isKey2Locked)
        {
            playerManager.currentWeapon = PlayerManager.WeaponType.Spear;
            ActivateObject(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && !isKey3Locked)
        {
            playerManager.currentWeapon = PlayerManager.WeaponType.Bow;
            ActivateObject(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            potionScript.UsePotion(); // 포션 사용            
            UpdatePotionCount();
            
        }
    }

    public void ActivateObject(int objectNumber)
    {
        // 모든 오브젝트 비활성화
        object1.SetActive(false);
        object2.SetActive(false);
        object3.SetActive(false);

        // 슬롯 원래 상태로 되돌리기
        ResetSlot(slot1);
        ResetSlot(slot2);
        ResetSlot(slot3);

        // 입력된 번호에 해당하는 오브젝트 활성화 및 슬롯 조정
        switch (objectNumber)
        {
            case 1:
                object1.SetActive(true);
                AdjustSlot(slot1, true);
                AdjustSlot(slot2, false);
                AdjustSlot(slot3, false);
                break;
            case 2:
                object2.SetActive(true);
                AdjustSlot(slot1, false);
                AdjustSlot(slot2, true);
                AdjustSlot(slot3, false);
                break;
            case 3:
                object3.SetActive(true);
                AdjustSlot(slot1, false);
                AdjustSlot(slot2, false);
                AdjustSlot(slot3, true);
                break;

        }
    }

    public void UnlockKey(int keyNumber)
    {
        // 키 잠금 해제
        if (keyNumber == 2)
        {
            isKey2Locked = false;
        }
        else if (keyNumber == 3)
        {
            isKey3Locked = false;
        }
    }

    void AdjustSlot(GameObject slot, bool isActive)
    {
        if (isActive)
        {
            slot.transform.localScale = originalScale * enlargedScaleFactor;
            SetSlotTransparency(slot, opaqueColor);
        }
        else
        {
            slot.transform.localScale = originalScale;
            SetSlotTransparency(slot, transparentColor);
        }
    }

    void ResetSlot(GameObject slot)
    {
        slot.transform.localScale = originalScale;
        SetSlotTransparency(slot, opaqueColor);
    }

    void SetSlotTransparency(GameObject slot, Color color)
    {
        Image[] images = slot.GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            image.color = color;
        }
    }
    public void UpdatePotionCount()
    {
        if (potionScript != null && potionCountText != null)
        {
            potionCountText.text = potionScript.potionCount.ToString(); // 포션 개수 업데이트
            potionCountText.enabled = potionScript.potionCount > 0; // 포션 개수가 0이면 텍스트 숨김
        }
    }
    void solt()
    {
        AdjustSlot(slot4, true);
    }
}

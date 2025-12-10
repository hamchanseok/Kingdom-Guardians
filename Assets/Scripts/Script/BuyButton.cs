using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyButton : MonoBehaviour
{
    public int price = 0; //상품 가격
    public int unlockkey; //잠금 해제할 키 번호

    public PlayerStats playerStats;
    public SlotManager slotManager;

    public GameObject getweapon; //버튼과 대응하는 무기 슬롯 
    public GameObject boughtitem; //"보유중" 버튼 이미지
    public GameObject cantbuy; //돈 부족하면 뜨는 텍스트

    public GameObject[] ArmorIcon;

    public Text priceText;

    private int armorlevel = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(priceText != null)
        {
            priceText.text = price.ToString();
        }
    }

    public void buy()
    {
        if (playerStats.curGold >= price)
        {
            playerStats.curGold = playerStats.curGold - price;

            getweapon.SetActive(true); //해당하는 무기 슬롯 활성화

            if (unlockkey != 4 && unlockkey != 5) //무기 아이템일때
            {
                slotManager.UnlockKey(unlockkey); //지정해 놓은 무기 슬롯 키 잠금해제
                gameObject.SetActive(false);
                boughtitem.SetActive(true);
            }

            if (unlockkey == 4) //포션일때
            {
                getweapon.SetActive(true);
                Potion potionScript = slotManager.potionScript;
                if (potionScript != null)
                {
                    potionScript.potionCount++; // 포션 개수 증가
                    slotManager.UpdatePotionCount(); // 포션 개수 UI 업데이트
                }
            }

            if (unlockkey == 5) //구매한 아이템이 갑옷일 때
            {
                switch (armorlevel)
                {
                    case 0:
                        playerStats.defenseRate = 0.2f;
                        price = 1000;
                        ArmorIcon[0].SetActive(false);
                        ArmorIcon[1].SetActive(true);
                        break;
                    case 1:
                        playerStats.defenseRate = 0.4f;
                        price = 1500;
                        ArmorIcon[1].SetActive(false) ;
                        ArmorIcon[2].SetActive(true);
                        break;
                    case 2:
                        playerStats.defenseRate = 0.6f;
                        gameObject.SetActive(false);
                        boughtitem.SetActive(true);
                        break;
                }
                armorlevel++;
            }

            

        }
        else
        {
            StartCoroutine(NotEnough());
        }
    }

    IEnumerator NotEnough()
    {
        cantbuy.SetActive(true);

        yield return new WaitForSeconds(1f);

        cantbuy.SetActive(false);
    }

    
}
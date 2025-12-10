using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : BaseStats
{
    public int curGold { get; set; }
    public override void InitStat() //BaseStats에 있는 Init스텟 재정의
    {
        szName = "Player";
        statMaxHP = 100;
        statCurHP = statMaxHP;
        curGold = 2000;
        defenseRate = 0; //방어율 처음에는 0퍼센트
        

        isDead = false;
        UIManager.s.UpdatePlayer(this);
    }

    protected override void UpdateAfterReceiveAttack() //// 체력이 0이 되면 죽는 메서드 사용
    {
        base.UpdateAfterReceiveAttack();
        Debug.Log(szName + "HP = " + statCurHP.ToString());
        UIManager.s.UpdatePlayer(this);

        
        StartCoroutine(UIManager.s.FadeRedScreenEffect());// 피격 효과
    }

    public void AddGold(int gold)
    {
        this.curGold += gold;
        UIManager.s.UpdatePlayer(this);
    }

    public bool SpendGold(int price) // 메서드의 매개변수 이름을 amount에서 price로 변경
    {
        if (curGold >= price) // amount를 price로 변경
        {
            curGold -= price; // amount를 price로 변경
            UIManager.s.UpdatePlayer(this);
            return true;
        }
        else
        {
            Debug.Log("Not enough gold.");
            return false;
        }
    }

    public void Heal(int amount)
    {
        statCurHP = Mathf.Min(statCurHP + amount, statMaxHP); // 최대 체력을 넘지 않도록 회복
        Debug.Log($"{szName} healed by {amount}. Current HP: {statCurHP}");
        UIManager.s.UpdatePlayer(this); // UI 업데이트
    }
}

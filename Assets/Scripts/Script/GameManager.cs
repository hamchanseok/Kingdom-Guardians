using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public SpawnManager spawnManager; // 스폰 매니저 참조
    public PlayerStats playerStats; // 플레이어 스탯 참조
    public int goldReward = 1500; // 스테이지 완료 보상
    public GameObject startPariticle;
    public GameObject shop;
    public UIManager UIManager;

    [HideInInspector] public int currentStage = 0;
    
    [HideInInspector] public bool restime = false;
    [HideInInspector] public bool isGameStarted = false;

    

    void Start()
    {
        startPariticle.SetActive(true);        
       
    }


    private void Update()
    {
       

        
    }
    public void StartStage()
    {
        spawnManager.StartStage(currentStage);
        UIManager.s.StageName();
        isGameStarted = true;
        restime = false;
        shop.SetActive(false);
        startPariticle.SetActive(false);
        StartCoroutine(CheckStageCompletion());
    }

    public void NextStage()
    {

        // 다음 스테이지로 이동
        currentStage++;
        if (currentStage < spawnManager.stageInfos.Length)
        {
            StartStage();
            startPariticle.SetActive(false);
        }
        else
        {
            Debug.Log("모든 스테이지를 완료했습니다!");
        }

    }

    public void EndStage()
    {
        restime = true;
        // 플레이어에게 보상 지급
        playerStats.AddGold(goldReward);
        startPariticle.SetActive(true);
        shop.SetActive(true);
    }


    IEnumerator CheckStageCompletion()
    {
        while (true)
        {
            if (AllMonstersDefeated())
            {
                EndStage();
                break;
            }
            yield return new WaitForSeconds(1f); // 1초마다 체크
        }
    }

    bool AllMonstersDefeated()
    {
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        return enemyCount == 0;
    }


}

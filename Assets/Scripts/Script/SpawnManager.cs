using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public StageInfo[] stageInfos; // 각 스테이지에 대한 정보
    public GameObject[] spawnPortals; // 스폰 위치 오브젝트 배열

    public void StartStage(int stageIndex)
    {
        StageInfo stageInfo = stageInfos[stageIndex];

        for (int i = 0; i < stageInfo.monsters.Length; i++)
        {
            GameObject monsterPrefab = stageInfo.monsters[i];
            int monsterCount = stageInfo.monsterCounts[i];

            for (int j = 0; j < monsterCount; j++)
            {
                // 몬스터를 생성합니다.
                SpawnMonster(monsterPrefab, spawnPortals[Random.Range(0, spawnPortals.Length)].transform.position);
            }
        }
    }

    void SpawnMonster(GameObject monsterPrefab, Vector3 spawnPosition)
    {
        // 몬스터를 생성합니다.
        Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
    }
}

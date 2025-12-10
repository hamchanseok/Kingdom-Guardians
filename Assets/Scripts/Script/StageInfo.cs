using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageInfo", menuName = "Stage/StageInfo", order = 1)]
public class StageInfo : ScriptableObject
{
    public GameObject[] monsters; // 각 스테이지에서 나오는 몬스터들
    public int[] monsterCounts; // 각 스테이지에서 나오는 몬스터들의 수
}

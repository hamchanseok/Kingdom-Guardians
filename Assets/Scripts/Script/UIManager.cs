using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UIManager : MonoBehaviour //플레이어 UI
{
    public static UIManager s;
    public GameManager gameManager;
    public PlayerStats playerStats;
    public Image playerHP;
    public Image redScreenEffect;
    public Text playerGold;
    public Text ArmorText;
    public Text leftEnemy;
    public GameObject key;
    public GameObject gameover;
    public GameObject clearScreen;
    public GameObject phase;

    


    private void Awake()
    {
        s = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ArmorText.text = (playerStats.defenseRate * 100).ToString() + "%";

        if(!gameManager.restime)
        {
            NavMeshAgent[] enemies = Object.FindObjectsOfType<NavMeshAgent>();
            leftEnemy.text = "남은 적 수: " + enemies.Length + "마리";
        }
        else
        {
            leftEnemy.text = "정비시간";
        }

        playerGold.text = "골드: " + playerStats.curGold.ToString();
    }

    public void UpdatePlayer(PlayerStats stats)
    { 
        playerHP.fillAmount = ((float)stats.statCurHP / (float)stats.statMaxHP);
        
    }

    public void CursorOn()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CursorOff()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public IEnumerator FadeRedScreenEffect() //피격 시 화면 테두리 붉어지는 코루틴 
    {
        
        // 페이드 인 (투명도 증가)
        float alpha = 0f;
        while (alpha < 1.0f)
        {
            alpha += Time.deltaTime; // 원하는 속도 조절
            redScreenEffect.color = new Color(1f, 0f, 0f, alpha);
            yield return null;
        }

        // 잠시 대기
        yield return new WaitForSeconds(0.5f);

        // 페이드 아웃 (투명도 감소)
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime; // 원하는 속도 조절
            redScreenEffect.color = new Color(1f, 0f, 0f, alpha);
            yield return null;
        }
    }

    public void GameOver()
    {
        gameover.SetActive(true);
    }

    public void Clear()
    {
        clearScreen.SetActive(true);
    }

    public void StageName()
    {
        StartCoroutine(StageTitle());
    }

    public IEnumerator StageTitle()
    {
        Text phaseText = phase.GetComponentInChildren<Text>();
        phaseText.text = "Phase " + (gameManager.currentStage + 1).ToString();

        phase.SetActive(true);

        yield return new WaitForSeconds(3f);

        phase.SetActive(false);  
    }

}

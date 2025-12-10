using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public bool IsPlayerInStartZone = false;
    public GameManager gm;

    public Text displayText; // 텍스트가 표시될 UI Text
    public GameObject talkingbox;
    public GameObject button;
    public float typingSpeed = 0.05f; // 글자당 타이핑 속도
    public string conversation; //npc와 대화내용
    private Coroutine typingCoroutine; // 타이핑 효과 Coroutine 제어
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        IsPlayerInStartZone = false;
    }
    // Update is called once per frame
    void Update()
    {
       if(IsPlayerInStartZone && !gm.isGameStarted && Input.GetKeyDown(KeyCode.F)) //NPC와 최초 대화 시 실행
       {
            UIManager.s.key.SetActive(false);
            talkingbox.SetActive(true);
            ShowTypingText();
            UIManager.s.CursorOn();
            CameraController.CameraFreeze = true;
        }
       else if(IsPlayerInStartZone && Input.GetKeyDown(KeyCode.F))
       {
            UIManager.s.key.SetActive(false);
            gm.NextStage();
       }
    }

    private void OnTriggerEnter(Collider other)
    {
        IsPlayerInStartZone = true;
        UIManager.s.key.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        IsPlayerInStartZone = false;
        UIManager.s.key.SetActive(false);
        UIManager.s.CursorOff();
        talkingbox.SetActive(false);
        CameraController.CameraFreeze = false;
    }

    public void yes()
    {
        gm.StartStage();
        talkingbox.SetActive(false);
        CameraController.CameraFreeze = false;
        UIManager.s.CursorOff();
    }

    public void no()
    {
        talkingbox.SetActive(false);
        CameraController.CameraFreeze = false;
        UIManager.s.CursorOff();
    }

    public void ShowTypingText()
    {
        // 기존에 실행 중인 타이핑 효과가 있다면 중지
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        // 새 타이핑 효과 시작
        typingCoroutine = StartCoroutine(TypeText());
    }

    // 타이핑 효과를 위한 Coroutine
    private IEnumerator TypeText()
    {
        displayText.text = ""; // 기존 텍스트 초기화
        foreach (char letter in conversation)
        {
            displayText.text += letter; // 한 글자씩 추가
            yield return new WaitForSeconds(typingSpeed); // 타이핑 속도만큼 대기
        }

        typingCoroutine = null; // 타이핑 완료 후 Coroutine 초기화
        button.SetActive(true);
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [SerializeField] private InputField chatInputField;
    [SerializeField] private Text chatDisplayText;
    [SerializeField] private ScrollRect scrollRect;

    private List<string> chatMessages = new List<string>(); // 채팅 메시지를 저장할 리스트
    private const int maxChatMessages = 25; // 최대 채팅 수 

    // Start is called before the first frame update
    void Start()
    {
        chatInputField.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            chatInputField.gameObject.SetActive(true);
            chatInputField.ActivateInputField();
        }

        if (chatInputField.isFocused && Input.GetKeyDown(KeyCode.Return))
        {
            SendChatMessage(chatInputField.text);
            chatInputField.text = "";
            chatInputField.DeactivateInputField();
        }
    }
    void SendChatMessage(String message)
    {
        if (string.IsNullOrEmpty(message))
            return;

        AddChatMessage("Me: " + message);

        var chatData = new CHAT_MESSAGE
        {
            USER = NetGameManager.instance.m_userHandle.m_szUserID,
            MESSAGE = message
        };

        string sendData = LitJson.JsonMapper.ToJson(chatData);
        NetGameManager.instance.RoomBroadcast(sendData);
    }

    public void AddChatMessage(string message)
    {
        if (chatMessages.Count >= maxChatMessages)
        {
            chatMessages.RemoveAt(0);
        }

        chatMessages.Add(message);
        UpdateChatDisplay();
    }

    private void UpdateChatDisplay()
    {
        chatDisplayText.text = "";

        foreach (string message in chatMessages)
        {
            chatDisplayText.text += message + "\n";
        }

        // 스크롤을 맨 아래로
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();
    }

    public void ReceiveChatMessage(string jsonData)
    {
        LitJson.JsonData data = LitJson.JsonMapper.ToObject(jsonData);
        string user = data["USER"].ToString();
        string message = data["MESSAGE"].ToString();

        AddChatMessage(user + ": " + message);
    }
}

// 채팅 메시지를 보내기 위한 클래스
[System.Serializable]
public class CHAT_MESSAGE
{
    public string USER;
    public string MESSAGE;
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MNF;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    public TextMesh userID;

    //이동 속도
    float moveSpeed = 10.0f;
    //회전 속도
    float rotateSpeed = 150.0f;

    NetVector3 prevTransform0 = new NetVector3(0,0,0);
    NetVector3 prevTransform1 = new NetVector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name != NetGameManager.instance.m_userHandle.m_szUserID)
            return;
        /*
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    transform.Translate(Vector3.left * 10.0f * Time.deltaTime);
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    transform.Translate(Vector3.right * 10.0f * Time.deltaTime);
                }
                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    transform.Translate(Vector3.forward * 10.0f * Time.deltaTime);
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    transform.Translate(Vector3.back * 10.0f * Time.deltaTime);
                }
        */
        //이동속도
        float move = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        //회전속도
        float rotate = Input.GetAxis("Horizontal") * rotateSpeed * Time.deltaTime;

        //이동
        transform.Translate(0, 0, move);
        //회전
        transform.Rotate(0, rotate, 0);

        //1007 - 위치와 각도를 저장하고 값이 다르면 전송
        prevTransform0 = new NetVector3(transform.position);
        prevTransform1 = new NetVector3(transform.rotation.eulerAngles);

        UserSession userSession = NetGameManager.instance.GetRoomUserSession(
            NetGameManager.instance.m_userHandle.m_szUserID);

        if ( prevTransform0.Equals(userSession.m_userTransform[0]) && prevTransform1.Equals(userSession.m_userTransform[1]))
            return;

        userSession.m_userTransform[0] = prevTransform0;
        userSession.m_userTransform[1] = prevTransform1;

        NetManager.instance.Send_ROOM_USER_MOVE_DIRECT(userSession);
    }

	public void Init(UserSession user)
	{
        gameObject.name = user.m_szUserID;
        userID.text = user.m_szUserID;
	}
}

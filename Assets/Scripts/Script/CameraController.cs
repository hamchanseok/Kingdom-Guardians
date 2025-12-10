using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public Vector3 offset; // 기본 카메라와 플레이어 사이의 거리
    public Vector3 zoomedOffset; // 줌 인 상태에서의 카메라 오프셋
    public float sensitivity = 0.1f; // 마우스 감도
    public float zoomSpeed = 5f; // 줌 속도


    private float pitch = 0f; // 상하 회전 각도
    private float yaw = 0f; // 좌우 회전 각도

    private Vector3 currentOffset; // 현재 적용할 오프셋
    private PlayerManager playerManager;

    [HideInInspector]
    public bool isZooming = false; // 줌 인 여부
    public static bool CameraFreeze = false;


    void Start()
    {
        // 초기 오프셋 설정
        offset = transform.position - player.position;
        currentOffset = offset;
        playerManager = player.GetComponent<PlayerManager>();
    }

    void Update()
    {
        if (playerManager.currentWeapon == PlayerManager.WeaponType.Bow)
        {
            // 우클릭 상태에 따라 줌 인/아웃 여부를 설정
            if (Input.GetMouseButtonDown(1)) // 우클릭 눌렀을 때
            {
                isZooming = true;
            }
            else if (Input.GetMouseButtonUp(1)) // 우클릭 뗐을 때
            {
                isZooming = false;
            }
        }

        if (!CameraFreeze)
        {
            // 줌 인 여부와 관계없이 마우스 입력으로 회전 각도 조정
            yaw += Input.GetAxis("Mouse X") * sensitivity;
            pitch -= Input.GetAxis("Mouse Y") * sensitivity;
            pitch = Mathf.Clamp(pitch, -35, 40); // 상하 회전 제한
        }
    }

    void LateUpdate()
    {
        // 카메라의 회전 설정
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // 줌 인 여부에 따라 오프셋 변경
        currentOffset = Vector3.Lerp(currentOffset, isZooming ? zoomedOffset : offset, Time.deltaTime * zoomSpeed);

        // 회전된 오프셋 적용
        Vector3 rotatedOffset = rotation * currentOffset;

        // 카메라 위치 설정
        transform.position = player.position + rotatedOffset;

        // 줌 인 상태에서 플레이어의 상대 위치 보정
        if (isZooming)
        {
            // 플레이어가 항상 왼쪽 아래에 위치하도록 보정된 LookAt 위치
            transform.LookAt(player.position + Vector3.up * 1.5f + rotation * Vector3.right * 1.4f);
        }
        else
        {
            // 기본 시점에서는 중앙을 약간 위쪽으로 바라보기
            transform.LookAt(player.position + Vector3.up * 1.5f);
        }
    }



}

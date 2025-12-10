using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Bow : MonoBehaviour
{
    public GameObject arrowPrefab; // 발사할 화살 프리팹
    public GameObject CrossHair;
    public Transform arrowSpawnPoint; // 화살이 생성될 위치
    public float arrowSpeed = 20f; // 화살 속도
    public AimConstraint upperBodyAimConstraint;
    public GameObject player;
    public CameraController cc;

    private AudioSource audiosource;

    void Start()
    {
        audiosource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (cc.isZooming) // 우클릭을 누르고 있는 동안
        {
           
            upperBodyAimConstraint.enabled = true;
            upperBodyAimConstraint.weight = 1;
            CrossHair.SetActive(true);

            
            // player를 카메라가 보는 방향으로 회전
            Vector3 targetDirection = Camera.main.transform.forward;
            targetDirection.y = 0; // y축 회전만 적용하도록 설정
            player.transform.rotation = Quaternion.LookRotation(targetDirection);
        }
        else if (!cc.isZooming) // 우클릭을 떼면
        {
           
            upperBodyAimConstraint.weight = 0;
            upperBodyAimConstraint.enabled = false;
            CrossHair.SetActive(false);
        }
    }

    public void ShootArrow()
    {
        audiosource.PlayOneShot(audiosource.clip);
        // 화살 생성 및 방향 설정
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);

        arrow.transform.Rotate(90, 0, 0);

        // 화살에 힘을 가해 앞으로 발사
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = arrowSpawnPoint.forward * arrowSpeed;
        }
    }
}

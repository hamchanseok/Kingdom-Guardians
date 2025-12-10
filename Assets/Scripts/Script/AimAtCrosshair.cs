using UnityEngine;

public class AimAtCrosshair : MonoBehaviour
{
    public Camera playerCamera; // 플레이어 카메라
    public Transform gunBarrel; // 총구 오브젝트
    public float maxDistance = 100f; // 레이캐스트 거리
    private CameraController cc;

    private void Start()
    {
        cc = playerCamera.GetComponent<CameraController>();
    }
    void Update()
    {
        if(cc.isZooming)
        {
            // 레이캐스트를 통해 카메라가 바라보는 지점 감지
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // 화면 중앙 조준선 위치
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                // 감지된 지점을 Aim Constraint의 타겟으로 설정
                gunBarrel.LookAt(hit.point);
            }
            else
            {
                // 감지된 지점이 없을 경우 멀리 있는 가상의 지점으로 설정
                gunBarrel.LookAt(ray.GetPoint(maxDistance));
            }
        }
        
    }
}

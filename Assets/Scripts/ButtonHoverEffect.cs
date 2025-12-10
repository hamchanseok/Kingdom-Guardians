using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1.2f); // 커질 크기
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale; // 원래 크기 저장
    }

    // 마우스가 버튼 위로 올라갔을 때 호출
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = hoverScale; // 크기 변경
    }

    // 마우스가 버튼에서 내려갔을 때 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale; // 원래 크기로 복귀
    }
}

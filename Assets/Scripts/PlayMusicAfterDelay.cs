using UnityEngine;
using System.Collections;

public class PlayMusicAfterDelay : MonoBehaviour
{
    public AudioSource audioSource; // 오디오 소스 컴포넌트를 연결합니다.
    public float delay = 3f; // 딜레이 시간 (3초)

    void Start()
    {
        StartCoroutine(PlayMusicAfterDelayCoroutine());
    }

    IEnumerator PlayMusicAfterDelayCoroutine()
    {
        yield return new WaitForSeconds(delay);
        audioSource.Play();
    }
}

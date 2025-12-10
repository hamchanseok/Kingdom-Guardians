using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // 이 메서드를 버튼 클릭 이벤트에 연결합니다.
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // 예시: 특정 씬을 로드하는 메서드
    public void LoadSpecificScene()
    {
        SceneManager.LoadScene("map"); // 로드하려는 씬의 이름으로 변경
    }
}

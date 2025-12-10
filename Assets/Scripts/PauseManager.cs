using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenu; // 일시정지 시 활성화할 오브젝트
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f; // 게임 일시정지
        CameraController.CameraFreeze = true;
        isPaused = true;
    }

    void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f; // 게임 재개
        CameraController.CameraFreeze = false;
        isPaused = false;
    }
}

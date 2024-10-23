using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

// Pause menu logic
public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu, _levelUI;

    public void PauseOn()
    {
        _levelUI.SetActive(false);
        _pauseMenu.SetActive(true);
        ArrowsInput.Reset();
        Time.timeScale = .0f;
        YandexGame.GameplayStop();
    }

    public void Play()
    {
        _levelUI.SetActive(true);
        _pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        YandexGame.GameplayStart();
    }

    public void Home()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

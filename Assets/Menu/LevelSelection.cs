using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

// Loads the desired level by pressing the button
public class LevelSelection : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameParams _gameParams;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void ChooseLevel()
    {
        _gameParams.LevelNum = int.Parse(eventSystem.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text) - 1; // Which level should we load
        SceneManager.LoadScene("Level");
    }
}

using UnityEngine;
using UnityEngine.UI;

// Logic of work of pages with levels
public class SelectPage : MonoBehaviour
{
    [SerializeField] private GameParams _gameParams;
    [SerializeField] private GameObject[] _menu = new GameObject[4];
    [SerializeField] private int _levelsOnPageNum;  // How many levels are displayed on one page

    private int _currentPage;

    private void Start()
    {
        _currentPage = 0;
        _gameParams.LoadProgress();
        CheckButtonsAccses();   // Check the availability of buttons depending on the game progress
    }

    public void NextPage(int currentPage)
    {
        if (currentPage >= _menu.Length - 1)
            return;

        // Activate next page and check buttons availability
        _menu[currentPage].SetActive(false);
        _menu[currentPage + 1].SetActive(true);
        _currentPage++;
        CheckButtonsAccses();
    }

    public void PrevPage(int currentPage)
    {
        if (currentPage < 1)
            return;

        _menu[currentPage].SetActive(false);
        _menu[currentPage - 1].SetActive(true);
        _currentPage--;
    }

    private void CheckButtonsAccses()
    {
        Component[] buttons = _menu[_currentPage].GetComponentsInChildren<Button>(); // Get all buttons on page
        int buttonId = 0;
        int buttonsForCheckNum = _currentPage < _menu.Length - 1 ? _levelsOnPageNum : _levelsOnPageNum - 1; // Last page hasn't next page button

        foreach (Button button in buttons)
        {
            if (buttonId > buttonsForCheckNum)
                break;

            button.interactable = (_gameParams.PlayerProgress >= _levelsOnPageNum * _currentPage + buttonId); // Check availability depending on the game progress
            buttonId++;
        }
    }
}

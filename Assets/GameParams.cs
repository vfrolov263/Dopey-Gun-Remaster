using UnityEngine;
using YG;

[CreateAssetMenu(fileName = "GameParams", menuName = "Scriptable Objects/GameParams")]

// Some key game parametrs
public class GameParams : ScriptableObject
{
    [SerializeField] private int _levelNum; // Current level
    [SerializeField] private int _levelsNum;    // Number of levels in the game
    [SerializeField] private int _playerProgress;

    private bool _advertisementOn = true; // Show advertisement flag

    public int LevelNum
    {
        set
        {
            _levelNum = value;

            if (_levelNum < 0)
                _levelNum = 0;
            else if (_levelNum >= _levelsNum)
                _levelNum = _levelsNum - 1;
        }
        get
        {
            return _levelNum;
        } 
    }

    public int PlayerProgress
    {
        get
        {
            return _playerProgress;
        } 
    }

    public bool IsEnd()
    {
        return _playerProgress == _levelsNum;   // Check for game finished
    }

    public void LevelPassed()
    {
        if (_playerProgress < _levelsNum)
        {
            // Save progress
            _playerProgress = _levelNum + 1;
            YandexGame.savesData.playerProgress = _playerProgress;
            YandexGame.SaveProgress();
        }
    }

    public void LoadProgress()
    {
        _playerProgress = YandexGame.savesData.playerProgress;
    }

    public void SetAdvertising(bool show)
    {
        _advertisementOn = show;
    }

    public bool IsAdvertisingOn()
    {
        return _advertisementOn;
    }
}

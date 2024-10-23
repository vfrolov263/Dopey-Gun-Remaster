using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System;
using YG;

// Load level map, arranges the objects
public class MapLoader : MonoBehaviour
{
    [SerializeField] private Tile _wallTile;
    [SerializeField] private Tile _floorTile;
    [SerializeField] private Tile _finishTile;
    [SerializeField] private Tile _reflectorMTile;
    [SerializeField] private Tile _reflectorPTile;
    [SerializeField] private Tilemap _tileMap;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Vector2 _playerOffset;
    [SerializeField] private GameObject _turretPrefab;
    [SerializeField] private GameObject _winPrefab;
    [SerializeField] private GameParams _gameParams;
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector2 _baseCameraCoef;
    [SerializeField] private Camera2DDinamicSize _cameraResizer;    // Provides optimal camera size for the level
    [SerializeField] private GameObject _winMenu, _endMenu;
    [SerializeField] private YandexGame _yg;    // For Yandex Games

    private char[,] _map = new char[40, 30];    // Level map
    public Vector2Int _minPoint = new Vector2Int(39, 29), _maxPoint = new Vector2Int(0, 0); // For check level size when loading it

    void OnEnable()
    {
        Time.timeScale = 1.0f;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (_gameParams.IsAdvertisingOn())
            _yg._FullscreenShow();  // Show advertisement when start level

        LoadMap($"Level_{_gameParams.LevelNum}");   // Load selected level
        ArrowsInput.Reset();
        _gameParams.SetAdvertising(true);   // Advertisment on and can be run on non-gaming actions (Yandex requirements)
        YandexGame.GameplayStart();
    }

    public char[,] GetMap()
    {
        return _map;
    }

    public void ReloadMap()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextMap()
    {
        _gameParams.LevelNum++;
        ReloadMap();
    }

    public void Win()
    {
        Instantiate(_winPrefab, new Vector3(.0f, .0f, .0f), Quaternion.Euler(.0f, .0f, .0f));
        _gameParams.LevelPassed();  // Save progress

        if (_gameParams.IsEnd())
            _endMenu.SetActive(true);   // Game finished
        else
            _winMenu.SetActive(true);   // Just another level passed

        YandexGame.GameplayStop();
    }

    private void LoadMap(string name)
    {
        TextAsset map = Resources.Load<TextAsset>(name);
        ParseMap(map.text);
    }

    // Parse txt file with level map
    private void ParseMap(string map)
    {
        string[] lines = map.Split('\n');

        for (int j = 0; j < lines.Length; j++)
        {
            string valueLine = lines[j];
            int realJ = lines.Length - j - 1;   // Convert j to Y coordinate axis

            for (int i = 0; i < valueLine.Length - 1; i++)
            {
                // Zero means empty space
                if (valueLine[i] != '0')
                {
                    AddTile(valueLine[i], new Vector3Int(i, realJ, 0));
                    CheckMapSize(i, realJ); // Calculate map size
                }

               _map[i, realJ] = valueLine[i];
            }
        }

        SetCamera();    // Set camera size for this level
    }

    // Calculate map size for set camera then
    private void CheckMapSize(int i, int j)
    {
        if (_maxPoint.x < i)
            _maxPoint.x = i;

        if (_minPoint.x > i)
            _minPoint.x = i;

        if (_maxPoint.y < j)
            _maxPoint.y = j;

        if (_minPoint.y > j)
            _minPoint.y = j;

    }

    // Set position and size of the camera
    private void SetCamera()
    {
        Vector2 centerOffset = new Vector2((float)(_maxPoint.x - _minPoint.x + 1.0f) / 2.0f, (float)(_maxPoint.y - _minPoint.y + 1.0f) / 2.0f);
        _camera.transform.position = new Vector3(_minPoint.x + centerOffset.x, _minPoint.y + centerOffset.y, -10.0f);
        _cameraResizer.SetLevelSizeInUnits(centerOffset.x * 2.0f, centerOffset.y * 2.0f);
    }

    // Add tiles:
    // # - wall
    // space - floor
    // f - finish
    // p - player
    // / - first type reflector
    // \ - second type reflector
    // u - turret shooting up
    // d - turret shooting down
    // r - turret shooting right
    // l - turret shooting left
    private void AddTile(char c, Vector3Int position)
    {
        switch (c)
        {
            case '#':
                _tileMap.SetTile(position, _wallTile);
                break;
            case ' ':
                _tileMap.SetTile(position, _floorTile);
                break;
            case 'f':
                _tileMap.SetTile(position, _finishTile);
                break;
            case 'p':
                _tileMap.SetTile(position, _floorTile);
                Instantiate(_playerPrefab, new Vector3(position.x + _playerOffset.x, position.y + _playerOffset.y, 0), Quaternion.identity);
                break;
            case '/':
                _tileMap.SetTile(position, _reflectorPTile);
                break;
            case '\\':
                _tileMap.SetTile(position, _reflectorMTile);
                break;
            case 'u':
                AddTurret(position, .0f);
                break;
            case 'd':
                AddTurret(position, 180.0f);
                break;
            case 'r':
                AddTurret(position, -90.0f);
                break;
            case 'l':
                AddTurret(position, 90.0f);
                break;
            default:
                break;
        }
    }

    private void AddTurret(Vector3Int position, float angle)
    {
        _tileMap.SetTile(position, _floorTile);
        Instantiate(_turretPrefab, new Vector3(position.x + _playerOffset.x, position.y + _playerOffset.y, 0), Quaternion.Euler(.0f, .0f, angle));
    }
}
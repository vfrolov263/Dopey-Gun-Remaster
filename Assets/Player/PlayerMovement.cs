using UnityEngine;

// Player movement and interaction logic
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private AudioSource _stepsSound;
    [SerializeField] private float _speed;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameParams _gameParams;

    private char[,] _map;
    private Vector3 _nextPosition;  // Movements in the game are discrete, on squares
    private bool _isMoving = false;
    private Vector2Int _movementDirection = new Vector2Int(0, 0);

    enum PlayerState : int
    {
        MoveDown = 0,
        MoveLeft,
        Idle,
        MoveRight,
        MoveUp,
        Win
    }

    private PlayerState _eState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        GameObject mapLoader = GameObject.Find(nameof(MapLoader));
        _map = mapLoader.GetComponent<MapLoader>().GetMap();
        _eState = PlayerState.Idle;
        _isMoving = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_eState == PlayerState.Win)
        {
            Moving();
            return;
        }

        CheckFinish();
        CheckInput();
        Moving();
    }

    private void CheckFinish()
    {
        if (_map[(int)transform.position.x, (int)transform.position.y] == 'f')
        {
            _eState = PlayerState.Win;
            _explosionPrefab = null;    // Player shouldn't bang on object destroy
            GameObject mapLoader = GameObject.Find(nameof(MapLoader));
            mapLoader.GetComponent<MapLoader>().Win();
        }

    }

    private void CheckInput()
    {
        if (_isMoving)  // When player already moves from one square to another
            return;

        float vert = ArrowsInput.GetVerticalAxis();
        float hor = ArrowsInput.GetHorizontalAxis();

        // Stop animation when player doesn't move
        if (vert == .0f && hor == .0f)
        {
            _animator.SetBool("Moving", _isMoving);
            return;
        }

        // Calculate next player position
        _nextPosition = transform.position;

        if (vert != .0f)
            _nextPosition.y = transform.position.y + vert;
        else
            _nextPosition.x = transform.position.x + hor;

        int i = (int)_nextPosition.x;
        int j = (int)_nextPosition.y;

        // Checking for obstructions in next position
        if (_map[i, j] != ' ' && _map[i, j] != 'f')
            return;

        _isMoving = true;
        _eState = (PlayerState)((int)PlayerState.Idle + i - (int)transform.position.x + (j - (int)transform.position.y) * 2); // Change player state to correct move
        _map[(int)transform.position.x, (int)transform.position.y] = ' ';   // Old position is now empty

        if (_map[i, j] != 'f')
            _map[i, j] = 'p';   // Next position is now player position

        StartMove();
    }

    private void DestroyPlayer()
    {
        Destroy(gameObject);
    }

    private void StartMove()
    {
        _stepsSound.Play();
        _animator.SetBool("Moving", _isMoving);

        // Choose direction of player
        switch (_eState)
        {
            case PlayerState.MoveUp:
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                break;
            case PlayerState.MoveDown:
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                break;
            case PlayerState.MoveRight:
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);
                break;
            case PlayerState.MoveLeft:
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
                break;
            default:
                break;
        }
    }

    private void Moving()
    {
        if (!_isMoving)
            return;

        transform.position = Vector3.MoveTowards(transform.position, _nextPosition, _speed * Time.deltaTime);

        // When reached desired position
        if (transform.position == _nextPosition)
        {
            _isMoving = false;
            _stepsSound.Stop();
        }
    }

    private void  OnApplicationQuit()
    {
        _explosionPrefab = null;
    }

    private void OnDestroy()
    {
        // If it's not a win or quit - blow up the player
        if (_explosionPrefab != null && Time.timeScale > .0f)
        {
            Instantiate(_explosionPrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(.0f, .0f, .0f));
            _gameParams.SetAdvertising(false);  // Don't show advertising, it's not non-game action
        }
    }
}
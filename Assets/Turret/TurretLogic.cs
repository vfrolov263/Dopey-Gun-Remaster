using UnityEngine;

// Provides turrets shooting
public class TurretLogic : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _fireSound;
    private char[,] _map;
    private Vector2Int _intPos;
    private char _me;   // Turret sybol on map

    enum TurretState : int
    {
        Ready,
        Pending,
        Destroyed
    }

    private TurretState _eState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        GameObject mapLoader = GameObject.Find(nameof(MapLoader));
        _map = mapLoader.GetComponent<MapLoader>().GetMap();    // Get level map
        _eState = TurretState.Ready;
        _intPos = new Vector2Int((int)transform.position.x, (int)transform.position.y); // Position in integer coords
        _me = _map[_intPos.x, _intPos.y];   // Symbol on map (u, d, l or r)
    }

    // Update is called once per frame
    private void Update()
    {
        switch (_eState)
        {
            case TurretState.Ready:
            case TurretState.Pending:
                CheckBang();
                CheckForTarget();
                break;
            default:
                break;
        }
    }

    // Check if we should bang this turret
    private void CheckBang()
    {
        if (_map[_intPos.x, _intPos.y] == 't')  // t means that bullet destroyed this turret
        {
            _map[_intPos.x, _intPos.y] = ' ';   // now it's a floor
            _eState = TurretState.Destroyed;
            Instantiate(_explosionPrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(.0f, .0f, .0f));
            Destroy(gameObject);
        }
    }

    // When player cross turret fire line
    private void CheckForTarget()
    {
        if (IsPlayerOnFireLine())
        {
            if (_eState == TurretState.Ready)
            {
                Shoot();
                _eState = TurretState.Pending;  // While player is on fire line turret shouldn't shoot again
            }
        }
        else if (_eState == TurretState.Pending)    // When player not on fire line turret is ready to shoot again
            _eState = TurretState.Ready;
    }

    private bool IsPlayerOnFireLine()
    {
        int i = _intPos.x, j = _intPos.y;
        int di = 0, dj = 0, ei = -1, ej = -1;

        // The check depends on the direction of fire of the turret
        switch (_me)
        {
            case 'u':
                dj = 1;
                ej = _map.GetLength(1);
                break;
            case 'd':
                dj = -1;
                ej = -1;
                break;
            case 'r':
                di = 1;
                ei = _map.GetLength(0);
                break;
            case 'l':
                di = -1;
                ei = -1;
                break;
            default:
                break;
        }

        // If player is on fire line and not hidden behind a shelter - player is on fire line
        for (i += di, j += dj; i != ei && j != ej; i += di, j += dj)
        {
            if (_map[i, j] == 'p')
                return true;

            if (_map[i, j] != ' ' && _map[i, j] != 'f')
                break;
        }

        return false;
    }

    private void Shoot()
    {
        _animator.Play("Fire");
        _fireSound.Play();
        Instantiate(_bulletPrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(.0f, .0f, .0f));
    }
}
using UnityEngine;
using System;

// Move bullet on the level
public class BulletMovement : MonoBehaviour
{
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private float _speed;
    [SerializeField] private float _reflectCoeff;   // Determines the degree of bullet distance from the center of the reflector at which the bullet changes direction
    [SerializeField] private AudioSource _reflectSound;
    private char[,] _map;   // Level map
    private Vector2Int _direction = new Vector2Int(0, 0);   // Direction of moving
    private Vector2Int _startPoint, _reflectStartPoint = new Vector2Int(0, 0);  // To record the starting position and the reflection start point
    private int _reflectType = 1;   // The game uses two types of reflectors
    private bool _isReflected;  // Flag for detect if bullet finish reflection process
    private bool _isDangerForTurret;

    enum BulletState
    {
        Fly,
        Reflecting,
        Destroyed
    }

    private BulletState _eState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        GameObject mapLoader = GameObject.Find(nameof(MapLoader));
        _map = mapLoader.GetComponent<MapLoader>().GetMap();    // Get level map for check interaction with other objects on stage
        _startPoint = new Vector2Int((int)transform.position.x, (int)transform.position.y); // Save start position
        ChooseDirection(_map[_startPoint.x, _startPoint.y]);    // Choose direction depending on start position
        _eState = BulletState.Fly;  // Start state is fly
        _isReflected = false;
        _isDangerForTurret = false; // Bullet shouldn't destroy parent turret on start
    }

    // Update is called once per frame
    private void Update()
    {
        switch (_eState)
        {
            case BulletState.Fly:
                transform.Translate(_speed * Time.deltaTime * _direction.x, _speed * Time.deltaTime * _direction.y, .0f);

                // Check to see if bullet damage can be enabled 
                if (!_isDangerForTurret)
                    CheckDangerForTurret();

                CheckCollision();
                break;
            case BulletState.Reflecting:
                transform.Translate(_speed * Time.deltaTime * _direction.x, _speed * Time.deltaTime * _direction.y, .0f);
                Reflect();
                break;
            case BulletState.Destroyed:
                break;
            default:
                break;

        }
    }

    private void CheckDangerForTurret()
    {
        Vector2Int curPoint = new Vector2Int((int)transform.position.x, (int)transform.position.y);

        // When bullet fly away from parent turret it becames dangerous for turrets
        if (curPoint != _startPoint)
            _isDangerForTurret = true;
    }

    // Check symbols meanings in MapLoader
    private void CheckCollision()
    {
        Vector2Int curPoint = new Vector2Int((int)transform.position.x, (int)transform.position.y);

        if (_map[curPoint.x, curPoint.y] == '#')    // When bullet collide with wall
            Bang();
        else if (_map[curPoint.x, curPoint.y] == '/')   // Reflector of first type
        {
            _reflectType = 1;
            _reflectStartPoint = curPoint;  // Save reflection start point
            _eState = BulletState.Reflecting;
        }
        else if (_map[curPoint.x, curPoint.y] == '\\')  // Reflector of second type
        {
            _reflectType = -1;
            _reflectStartPoint = curPoint;  // Save reflection start point
            _eState = BulletState.Reflecting;
        }
        // When bullet collide with some turret
        else if (_isDangerForTurret && _map[curPoint.x, curPoint.y] != ' ' && _map[curPoint.x, curPoint.y] != 'p' && _map[curPoint.x, curPoint.y] != 'f')
        {
            _map[curPoint.x, curPoint.y] = 't'; // Let the record show that the turret here has been destroyed
            Bang();
        }

    }

    // Provides trajectory on reflection 
    private void Reflect()
    {
        // When there was a reflection off the reflector.
        if (_isReflected)
        {
            // Record the current position of the bullet on the map
            Vector2Int curPoint = new Vector2Int((int)transform.position.x, (int)transform.position.y);

            // If the bullet has left the field with the reflector - the reflection process is complete
            if (curPoint != _reflectStartPoint)
            {
                _isReflected = false;
                _eState = BulletState.Fly;
            }
        }
        else
        {
            float pos;

            // Record the position in the axis of motion
            if (_direction.x != 0)
                pos = transform.position.x;
            else
                pos = transform.position.y;

            // When the bullet reaches the center of the reflector, taking into account the coeff factor
            if (Math.Abs(0.5f - pos + Math.Truncate(pos)) < _reflectCoeff)
            {
                // Put it in the center
                transform.position = new Vector3((float)Math.Truncate(transform.position.x) + 0.5f, (float)Math.Truncate(transform.position.y) + 0.5f, transform.position.z);

                // We change the direction depending on the type of reflector and note that the reflection has taken place
                int d = _direction.x;
                _direction.x = _reflectType * _direction.y;
                _direction.y = _reflectType * d;
                _reflectSound.Play();
                _isReflected = true;
            }
        }
    }

    private void Bang()
    {
        _eState = BulletState.Destroyed;
        Instantiate(_explosionPrefab, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.Euler(.0f, .0f, .0f)); // Activate explosion
        Destroy(gameObject);
    }

    // Adjusts the initial motion vector
    private void ChooseDirection(char c)
    {
        switch (c)
        {
            case 'u':
                _direction.y = 1;
                break;
            case 'd':
                _direction.y = -1;
                break;
            case 'r':
                _direction.x = 1;
                break;
            case 'l':
                _direction.x = -1;
                break;
            default:
                break;
        }    
    }

    // Provide collisions with other bullets
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject);
        Bang();
    }
}

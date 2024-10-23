using UnityEngine;

/// <summary>
/// Depending on the aspect ratio of the level, it fits it into the moving, changing dimensions of the window.
/// </summary>
[RequireComponent(typeof(Camera))]
public class Camera2DDinamicSize : MonoBehaviour
{  
    [SerializeField] private Vector2 levelSizeInUnits = new Vector2(1.0f, 1.0f);
    private float lvlAspect; // Aspect ratio of level width and height

    private Camera componentCamera;
    private Vector2Int _lastScreenSize; // For checking of screen resize

    public void SetLevelSizeInUnits(float widht, float height)
    {
        levelSizeInUnits.x = widht;
        levelSizeInUnits.y = height;
        lvlAspect = levelSizeInUnits.x / levelSizeInUnits.y;
        ResizeCamera();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        componentCamera = GetComponent<Camera>();
        SetLevelSizeInUnits(levelSizeInUnits.x, levelSizeInUnits.y);
    }

    // Update is called once per frame
    private void Update()
    {
         if (_lastScreenSize.x != Screen.width || _lastScreenSize.y != Screen.height)
            ResizeCamera();

    }

    // Scales the camera so that the level fits the screen resolution as tightly as possible
    private void ResizeCamera()
    {
        if (!componentCamera)
            return;
            
        // Determine whether we fit it into the window - width or height
        if (componentCamera.aspect < lvlAspect)
            componentCamera.orthographicSize = levelSizeInUnits.x / componentCamera.aspect / 2.0f;
        else
            componentCamera.orthographicSize = levelSizeInUnits.y / 2.0f;
            
        _lastScreenSize.x = Screen.width;
        _lastScreenSize.y = Screen.height;
    }
}

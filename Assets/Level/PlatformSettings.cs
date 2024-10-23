using UnityEngine;

// Switches the interface to the right platform
public class PlatformSettings : MonoBehaviour
{
    [SerializeField] private GameObject[] _onlyDescktopObjects = new GameObject[2];
    [SerializeField] private GameObject[] _onlyAndroidObjects = new GameObject[2];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (Application.isMobilePlatform)
        {
            for (int i = 0; i < _onlyDescktopObjects.Length; i++)
                _onlyDescktopObjects[i].SetActive(false);

            for (int i = 0; i < _onlyAndroidObjects.Length; i++)
                _onlyAndroidObjects[i].SetActive(true);
        }
    }
}

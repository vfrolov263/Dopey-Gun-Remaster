using UnityEngine;

// Provides player lose
public class PlayerBang : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 1.5f);
    }

    private void OnDestroy()
    {
        GameObject mapLoader = GameObject.Find(nameof(MapLoader));

        if (mapLoader != null)
            mapLoader.GetComponent<MapLoader>().ReloadMap();
    }
}

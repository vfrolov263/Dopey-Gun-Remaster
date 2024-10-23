using UnityEngine;

// Provides turret bang animation life time
public class DestroyAfterBang : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 1.5f);
    }
}

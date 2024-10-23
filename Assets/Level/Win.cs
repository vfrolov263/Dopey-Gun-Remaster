using UnityEngine;

// Win menu timelife
public class Win : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Destroy(gameObject, 1.5f);
    }
}

using UnityEngine;

// Handling an esc key or pause button press
public class EscapeLogic : MonoBehaviour
{
    [SerializeField] private GameObject _levelUI;
    [SerializeField] private Pause _pause;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
         {
            if (_levelUI.active)
                _pause.PauseOn();
            else
                _pause.Play();
         }
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Provides mobile arrow controls
public class MobileController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Vector2 _direction;

    public void OnPointerDown(PointerEventData eventData)
    {
        ArrowsInput.Move(_direction);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ArrowsInput.ResetKey(_direction);
    }
}

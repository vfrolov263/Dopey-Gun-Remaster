using UnityEngine;

// Checks for motion button presses, universal for desktop version and mobile device 
public static class ArrowsInput
{
    private static Vector2 _movement = new Vector2(.0f, .0f);   // Movement vector 
    private static Vector2 _prevMovement = new Vector2(.0f, .0f);   // To record the previous key press. Allows you to hold down two keys at the same time

    public static float GetVerticalAxis()
    {
        float vertical = _movement.y != .0f ? _movement.y : Input.GetAxisRaw("Vertical");   // Handles both buttons and keyboard input
        return vertical;
    }

    public static float GetHorizontalAxis()
    {
        float horizontal = _movement.x != .0f ? _movement.x : Input.GetAxisRaw("Horizontal");   // Handles both buttons and keyboard input
        return horizontal;
    }

    // Use when pause game
    public static void Reset()
    {
        _prevMovement.x = .0f;
        _prevMovement.y = .0f;
        _movement = _prevMovement;
    }

    // Reset a specific button 
    public static void ResetKey(Vector2 direction)
    {
        // The key corresponds to the last pressed key (i.e. the current movement vector)
        if (_movement == direction)
        {
            _movement = _prevMovement;
            _prevMovement.x = .0f;
            _prevMovement.y = .0f;
        }
        // The key corresponds to the penultimate key pressed
        else if (_prevMovement == direction)
        {
            _prevMovement.x = .0f;
            _prevMovement.y = .0f;
        }
    }

    // Processes a new press by memorizing the currently pressed key as the previous key. This allows you to process two keystrokes at the same time
    public static void Move(Vector2 direction)
    {
        if (_prevMovement.x == .0f && _prevMovement.y == .0f)
        {
            _prevMovement = _movement;
            _movement = direction;
        }
    }
}

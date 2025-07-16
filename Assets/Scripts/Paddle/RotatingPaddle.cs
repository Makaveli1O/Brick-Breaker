using UnityEngine;
using UnityEngine.InputSystem;

public class RotatingPaddle : MonoBehaviour, IPaddleBehaviour
{
    private const float defaultAngle = 0f;
    private const float rotateLeftAngle = -45f;
    private const float rotateRightAngle = 45f;
    private const float speed = 5f;
    public float Speed => speed;

    private Rigidbody2D _rb;
    private float _targetAngle = defaultAngle;

    void Awake()
    {
        _rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    public void Action(InputAction.CallbackContext ctx)
    {
        Vector2 rotationInput = ctx.ReadValue<Vector2>();

        if (ctx.performed)
        {
            if (rotationInput == Vector2.left)
                _targetAngle = rotateRightAngle;
            else if (rotationInput == Vector2.right)
                _targetAngle = rotateLeftAngle;
        }
        else if (ctx.canceled)
        {
            _targetAngle = defaultAngle;
        }
    }

    void FixedUpdate()
    {
        // Smoothly rotate towards target angle
        float newAngle = Mathf.MoveTowardsAngle(
            _rb.rotation,
            _targetAngle,
            speed * 100f * Time.fixedDeltaTime
        );

        _rb.MoveRotation(newAngle);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class RotatingPaddle : MonoBehaviour, IPaddleBehaviour
{
    private const float _defaultAngle = 0f;
    private const float _rotateLeftAngle = -45f;
    private const float _rotateRightAngle = 45f;
    private float _speed = 5f;
    private float _acceleration = 20f;

    private Rigidbody2D _rb;
    private float _targetAngle = _defaultAngle;

    public float Speed { get => _speed; set => _speed = value; }
    public float Acceleration { get => _acceleration; set => _acceleration = value; }

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
                _targetAngle = _rotateRightAngle;
            else if (rotationInput == Vector2.right)
                _targetAngle = _rotateLeftAngle;
        }
        else if (ctx.canceled)
        {
            _targetAngle = _defaultAngle;
        }
    }

    void FixedUpdate()
    {
        // Smoothly rotate towards target angle
        float newAngle = Mathf.MoveTowardsAngle(
            _rb.rotation,
            _targetAngle,
            _speed * 100f * Time.fixedDeltaTime
        );

        _rb.MoveRotation(newAngle);
    }
}

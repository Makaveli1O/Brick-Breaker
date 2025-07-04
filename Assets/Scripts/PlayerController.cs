using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _paddlePrefab;
    private GameObject _paddleInstance;
    private IPaddleBehaviour _paddle;
    private PlayerControls _playerControls;
    private Vector2 _movementVector;
    [SerializeField] private float acceleration = 30f;
    [SerializeField] private PlayerParticleController _ppc;
    private float _verticalBoundary;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _rb.freezeRotation = true;
    }

    void Start()
    {
        _verticalBoundary = CalculateYBoundary();

        _paddleInstance = Instantiate(_paddlePrefab, transform.position, Quaternion.identity, transform);
        _paddle = _paddleInstance.GetComponent<IPaddleBehaviour>();

        if (_paddle == null)
            throw new Exception("IPaddleBehaviour not implemented on the paddle GameObject.");
    }

    void FixedUpdate()
    {
        float targetYSpeed = _movementVector.y * _paddle.Speed;
        float currentYSpeed = _rb.linearVelocity.y;

        float newYSpeed = Mathf.MoveTowards(currentYSpeed, targetYSpeed, acceleration * Time.fixedDeltaTime);
        _rb.linearVelocity = new Vector2(0f, newYSpeed);

        ClampToVerticalBounds();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            _movementVector = ctx.ReadValue<Vector2>();
            if (_movementVector.Equals(Vector2.down))
            {
                _ppc.StartUpwardThrust();
            }
            else if (_movementVector.Equals(Vector2.up))
            {
                _ppc.StartDownwardThrust();
            }
        }
        else if (ctx.canceled)
        {
            _movementVector = Vector2.zero;
            _ppc.StopBothThrusts();
        }
    }

    public void OnRotate(InputAction.CallbackContext ctx)
    {
        _paddle.Action(ctx);
    }

    private void ClampToVerticalBounds()
    {
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(pos.y, -_verticalBoundary, _verticalBoundary);
        transform.position = pos;
    }


    private float CalculateYBoundary()
    {
        float camHeight = Camera.main.orthographicSize;
        float hudOffset = LevelBounds.GetHudOffsetInUnits();
        float halfPaddleHeight = _paddlePrefab.GetComponent<SpriteRenderer>().bounds.extents.y;
        return camHeight - hudOffset - halfPaddleHeight;
    }

}

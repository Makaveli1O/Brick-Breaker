using System;
using Assets.Scripts.Ball;
using Assets.Scripts.GameHandler;
using Assets.Scripts.Powerups;
using Assets.Scripts.SharedKernel;
using Assets.Scripts.UI;
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
    [SerializeField] private PlayerParticleController _ppc;
    [SerializeField] private IBallController _ballController;
    public PowerupSpawner _powerupSpawner;
    private Rigidbody2D _rb;
    private bool _ballLaunched = false;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        _rb.freezeRotation = true;
    }

    void Start()
    {
        _paddleInstance = Instantiate(_paddlePrefab, transform.position, Quaternion.identity, transform);
        _paddle = _paddleInstance.GetComponent<IPaddleBehaviour>();
        _ballController = SimpleServiceLocator.Resolve<IBallController>();

        if (_paddle == null)
            throw new Exception("IPaddleBehaviour not implemented on the paddle GameObject.");
    }

    void FixedUpdate()
    {
        ManuelSteering();
    }

    private void ManuelSteering()
    {
        if (_movementVector.y == 0f) return;

        Vector3 pos = transform.position;

        float deltaY = _movementVector.y * _paddle.Speed * Time.fixedDeltaTime;
        pos.y += deltaY;

        float camHeight = Camera.main.orthographicSize;
        float hudOffset = LevelBounds.GetHudOffsetInUnits();
        float halfPaddleHeight = _paddlePrefab.GetComponent<SpriteRenderer>().bounds.extents.y;

        float upperBound = camHeight - hudOffset - halfPaddleHeight;
        float lowerBound = -camHeight + halfPaddleHeight;

        pos.y = Mathf.Clamp(pos.y, lowerBound, upperBound);
        _rb.MovePosition(pos); // this works with kinematic too
    }


    public void OnLaunchBall(InputAction.CallbackContext ctx)
    {
        // TODO own key binding 
        if (ctx.performed)
        {
            _powerupSpawner.Spawn(PowerupSpawner.PowerupTypes.SpeedBoost, _paddleInstance);
        }

        if (ctx.performed && !_ballLaunched)
        {
            _ballController.LaunchBall();
            _ballLaunched = true;
            SimpleServiceLocator.Resolve<InstructionsUI>().Hide();
            SimpleServiceLocator.Resolve<IGameStateController>().SetState(GameState.Playing);
        }
    }

    public void OnPauseGame(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            SimpleServiceLocator.Resolve<IPauseController>().TogglePause();
        }
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

    [Obsolete("Not used any more", true)]
    private void ClampToVerticalBounds()
    {
        Vector3 pos = transform.position;

        float camHeight = Camera.main.orthographicSize;
        float hudOffset = LevelBounds.GetHudOffsetInUnits();
        float halfPaddleHeight = _paddlePrefab.GetComponent<SpriteRenderer>().bounds.extents.y;

        float upperBound = camHeight - hudOffset - halfPaddleHeight;
        float lowerBound = -camHeight + halfPaddleHeight;

        pos.y = Mathf.Clamp(pos.y, lowerBound, upperBound);
        _rb.position = pos;
    }

    [Obsolete("Not used any more", true)]
    private void SimulatedSteering()
    {
        float currentYSpeed = _rb.linearVelocity.y;

        float targetYSpeed = _movementVector.y * _paddle.Speed;
        float smoothing = _paddle.Acceleration;

        _rb.linearVelocityY = Mathf.Lerp(currentYSpeed, targetYSpeed, smoothing * Time.fixedDeltaTime);

    }

    [Obsolete("Not used any more", true)]
    private void ArcadeSteering()
    {
        float currentYSpeed = _rb.linearVelocity.y;

        if (_movementVector.y == 0)
            _rb.linearVelocityY = 0f;
        else
        {
            float targetYSpeed = _movementVector.y * _paddle.Speed;
            float newYSpeed = Mathf.MoveTowards(currentYSpeed, targetYSpeed, _paddle.Acceleration * Time.fixedDeltaTime);
            _rb.linearVelocityY = newYSpeed;
        }
    }
}

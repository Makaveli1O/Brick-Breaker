using Assets.Scripts.SharedKernel;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

namespace Assets.Scripts.Ball
{
    public class BallController : MonoBehaviour, IBallController
    {
        [SerializeField] private AudioClip _launchBallClip;
        [SerializeField] private AudioClip _ballHit;
        private float _initialPush = 200f;
        private float _maxSpeed = 6f;
        private float _minSpeed = 4f;
        public bool IsSlowed { get; set; } = false;
        private ISoundPlayer _soundPlayer;
        public bool IsMoving => _rb.linearVelocity.sqrMagnitude > 0.001f;

        private Rigidbody2D _rb;

#if UNITY_EDITOR
        [SerializeField] private float _currentSpeed;
        void Update()
        {
            _currentSpeed = _rb.linearVelocity.magnitude;
        }
#endif

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _soundPlayer = SimpleServiceLocator.Resolve<ISoundPlayer>();
        }

        void FixedUpdate()
        {
            HandleMinAndMaxVelocity();
        }

        public void LaunchBall()
        {
            // Threshold used for the initial bump
            float threshold = .1f;

            float x = -1f; // Always launch to the left
            float y = Random.value < 0.5f ?
                Random.Range(-threshold, 0.5f) :
                Random.Range(0.5f, threshold);

            Vector2 direction = new Vector2(x, y).normalized;
            _rb.AddForce(direction * _initialPush);
            _soundPlayer.PlaySfx(_launchBallClip);
        }

        public void LaunchBall(Vector2 direction)
        {
            _rb.AddForce(direction * _initialPush);
            _soundPlayer.PlaySfx(_launchBallClip);
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (_rb.linearVelocity.magnitude >= _maxSpeed) return;
                _rb.AddForce(collision.relativeVelocity.normalized);
            }

            if (collision.gameObject.tag.Contains("Wall") && ShouldNudge()) NudgeDirection();

            _soundPlayer.PlaySfx(_ballHit);
        }

        private void HandleMinAndMaxVelocity()
        {
            if (_rb.linearVelocity.magnitude > _maxSpeed)
                _rb.linearVelocity = Vector3.ClampMagnitude(_rb.linearVelocity, _maxSpeed);
            else if (_rb.linearVelocity.magnitude < _minSpeed)
                _rb.linearVelocity = _rb.linearVelocity.normalized * _minSpeed;
        }

        // This functon should prevent ball from stucking between walls infinitely
        private void NudgeDirection()
        {
            float angleOffset = Random.Range(-5f, 5f);
            Quaternion rotation = Quaternion.Euler(0, 0, angleOffset);
            _rb.linearVelocity = rotation * _rb.linearVelocity.normalized * _rb.linearVelocity.magnitude;
        }
        
        // Check whether or not should nudge direction ( if near perfect angles )
        private bool ShouldNudge()
        {
            Vector2 v = _rb.linearVelocity.normalized;
            float dotX = Mathf.Abs(Vector2.Dot(v, Vector2.right));
            float dotY = Mathf.Abs(Vector2.Dot(v, Vector2.up));

            // Close to axis-aligned (dot near 1)
            return dotX > 0.995f || dotY > 0.995f;
        }
    }
}
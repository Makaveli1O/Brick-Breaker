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
        private float initialSpeed = 300f;
        private float maxSpeed = 1500f;
        private float speedIncreaseFactor = 1.5f;
        private ISoundPlayer _soundPlayer;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _soundPlayer = SimpleServiceLocator.Resolve<ISoundPlayer>();
        }

        public void LaunchBall()
        {
            float x = -1f; // always launch to the left
            float y = Random.value < 0.5f ?
                Random.Range(-1.0f, 0.5f) :
                Random.Range(0.5f, 1.0f);

            // Set initial velocity
            Vector2 direction = new Vector2(x, y).normalized;
            _rb.AddForce(direction * initialSpeed);
            _soundPlayer.PlaySfx(_launchBallClip);
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (_rb.linearVelocity.magnitude >= maxSpeed)
                {
                    Debug.Log("Max speed reached, no further increase.");
                    return;
                }
                _rb.AddForce(collision.relativeVelocity.normalized * speedIncreaseFactor);
            }

            _soundPlayer.PlaySfx(_ballHit);
        }
    }
}
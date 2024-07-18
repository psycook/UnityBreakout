using Unity.VisualScripting;
using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    [SerializeField]
    private float startSpeed = 5.0f;
    [SerializeField]
    private AudioClip batHit;
    [SerializeField]
    private AudioClip wallHit;
    [SerializeField]
    private AudioClip ballFail;
    [SerializeField]
    private AudioClip brickHit;

    private float _speed;
    private Rigidbody2D _rigidBody;
    private GameBehaviour _gameBehaviour;
    private PaddleBehaviour _paddleBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        _gameBehaviour = FindAnyObjectByType<GameBehaviour>();
        _paddleBehaviour = FindAnyObjectByType<PaddleBehaviour>();
        _rigidBody = GetComponent<Rigidbody2D>();
        Reset();
    }

    public void Reset()
    {
        transform.position = new Vector2(transform.position.x, -4.16f);
        _rigidBody.velocity = new Vector2(0.0f, 0.0f);

        if(_gameBehaviour.gameState == GameState.GameOver)
        {
            gameObject.SetActive(false);
        }
        else if (_gameBehaviour.gameState == GameState.Playing)
        {
            gameObject.SetActive(true);
            _speed = startSpeed;
            float angle = 25f; // initial angle 25 degrees
            Vector2 direction = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
            _rigidBody.velocity = direction * _speed;
        }
    }

    public void Freeze()
    {
        _rigidBody.velocity = new Vector2(0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(_gameBehaviour != null && _gameBehaviour.gameState == GameState.Serving)
        {
            if(_paddleBehaviour != null)
            {
                transform.position = new Vector2(_paddleBehaviour.transform.position.x, transform.position.y);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Brick")
        {
            if(brickHit != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound(brickHit, 1.0f);
            }
            _speed += 0.02f;
        }
        else if (collision.gameObject.tag == "Brick7")
        {
            if (brickHit != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound(brickHit, 1.0f);
            }
        }
        else if (collision.gameObject.tag == "Wall")
        {
            if (wallHit != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound(wallHit, 1.0f);
            }
            if (Mathf.Abs(_rigidBody.velocity.x) < 0.25)
            {
                if(_rigidBody.velocity.x >= 0.0f)
                {
                    _rigidBody.velocity = new Vector2(0.25f, _rigidBody.velocity.y);
                }
                else
                {
                    _rigidBody.velocity = new Vector2(-0.25f, _rigidBody.velocity.y);
                }
            }
        }
        else if (collision.gameObject.tag == "Player")
        {
            if (batHit != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound(batHit, 1.0f);
            }
            float positionDifference = transform.position.x - collision.gameObject.transform.position.x;
            Vector2 newDirection = new Vector2(positionDifference*5.0f, _rigidBody.velocity.y).normalized;
            _rigidBody.velocity = newDirection * _speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "OutOfBounds")
        {
            _gameBehaviour.BallLost();
        }
    }
}
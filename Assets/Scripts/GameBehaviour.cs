using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEditor.Profiling.Memory.Experimental;

public class GameBehaviour : MonoBehaviour
{
    [SerializeField]
    [Range(1,7)]
    private int lives;
    [SerializeField]
    private int score;
    [SerializeField]
    private TextMeshProUGUI livesText;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI levelInformationText;
    [SerializeField]
    private AudioClip levelWonClip;
    [SerializeField]
    private AudioClip GameOverClip;
    [SerializeField]
    private AudioClip BallFailClip;

    [SerializeField]
    private LevelBehaviour _levelBehaviour;

    private BallBehaviour _ballBehaviour;

    private PlayerInput _playerInput;
    private InputAction _buttonAction;

    private GameState _gameState = GameState.Idle;

    public GameState gameState
    {
        get { return _gameState; }
        set
        {
            _gameState = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _ballBehaviour = FindAnyObjectByType<BallBehaviour>();
        _playerInput = GetComponent<PlayerInput>();
        _buttonAction = _playerInput.actions["Buttons"];
        if(_buttonAction != null)
        {
            _buttonAction.Enable();
            _buttonAction.performed += ButtonPressed;
        }

        if (livesText != null)
        {
            livesText.text = $"LIVES\n{lives.ToString("D2")}";
        }
        if (scoreText != null)
        {
            scoreText.text = $"SCORE\n{score.ToString("D6")}";
        }
        if (levelText != null && _levelBehaviour != null)
        {
            _levelBehaviour.newGame();
            levelText.text = $"LEVEL\n{_levelBehaviour.getDisplayLevel().ToString("D2")}";
        }
        gameState = GameState.Idle;
        StartLevel();
        gameState = GameState.Serving;
    }

    private void ButtonPressed(InputAction.CallbackContext context)
    {
        Debug.Log($"ButtonPressed {context.control.name}");

        if((context.control.name == "buttonSouth" || context.control.name == "space") && gameState == GameState.Serving)
            {
            gameState = GameState.Playing;
            if (levelInformationText != null)
            {
                levelInformationText.enabled = false;
            }
            BallBehaviour ballBehaviour = FindAnyObjectByType<BallBehaviour>();
            if(ballBehaviour != null)
            {
                ballBehaviour.Reset();
            }
        }

        if((context.control.name == "s"))
        {
            // clear all the existing bricks
            GameObject Level = GameObject.Find("LevelBehaviour");
            // delete all children with the tag "Brick"
            foreach (Transform child in Level.transform)
            {
                if (child.tag == "Brick")
                {
                    Destroy(child.gameObject);
                }
            }
            levelWon();
        }
    }

    public void BallLost()
    {
        lives--;

        Debug.Log($"Lives {lives}");

        if (livesText != null)
        {
            livesText.text = $"LIVES\n{lives.ToString("D2")}";
        }

        if (lives == 0)
        {
            gameState = GameState.GameOver;
            levelInformationText.text = $"GAME OVER";
            levelInformationText.enabled = true;
            if(GameOverClip != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound(GameOverClip, 1.0f);
            }
            
            // Wait for 2 seconds before moving to the next level
            Invoke("segueToIntroScene", 5.0f);
        }
        else
        {
            if (BallFailClip != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound(BallFailClip, 0.25f);
            }
            gameState = GameState.Serving;
            FindAnyObjectByType<BallBehaviour>().Reset();
        }
    }

    private void segueToIntroScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("IntroScene");
    }

    private void StartLevel()
    {
        if (levelInformationText != null)
        {
            levelInformationText.enabled = true;
            levelInformationText.text = $"Level {_levelBehaviour.getDisplayLevel().ToString("D2")} - GET READY!";
        }
        if(_levelBehaviour != null)
        {
            _levelBehaviour.startLevel();
        }
    }

    public void IncrementScore(int value)
    {
        score += value;
        if(scoreText != null)
        {
            scoreText.text = $"SCORE\n{score.ToString("D6")}";
        }
    }
    private void OnDestroy()
    {
        if (_buttonAction != null)
        {
            _buttonAction.performed -= ButtonPressed;
            _buttonAction.Disable();
        }
    }

    public void levelWon()
    {
        if(AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(levelWonClip, 1.0f);
        }
        levelInformationText.text = $"Level {_levelBehaviour.getDisplayLevel().ToString("D2")} - LEVEL COMPLETE!";
        levelInformationText.enabled = true;
        if(_ballBehaviour != null)
        {
            _ballBehaviour.Freeze();
        }

        // Wait for 2 seconds before moving to the next level
        Invoke("segueToNextLevel", 2.0f);
    }

    public void segueToNextLevel()
    {
        if(_levelBehaviour != null)
        {
            var hasNextLevel = _levelBehaviour.nextLevel();
            if (!hasNextLevel)
            {
                Debug.Log("Game Won");
                gameState = GameState.GameWon;
                levelInformationText.text = "Congratulations - Game Won";
                levelInformationText.enabled = true;
                return;
            }
            levelText.text = $"LEVEL\n{_levelBehaviour.getDisplayLevel().ToString("D2")}";
        }
        gameState = GameState.Idle;
        _ballBehaviour.Reset();
        StartLevel();
        gameState = GameState.Serving;
    }
}
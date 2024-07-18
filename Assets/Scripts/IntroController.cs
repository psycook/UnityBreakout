using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class IntroController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private InputAction _buttonAction;

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _buttonAction = _playerInput.actions["Buttons"];
        if (_buttonAction != null)
        {
            _buttonAction.Enable();
            _buttonAction.performed += ButtonPressed;
        }
    }

    private void ButtonPressed(InputAction.CallbackContext context)
    {
        Debug.Log($"IntroController:ButtonPressed {context.control.name}");

        if (
            (context.control.name == "buttonSouth" ||
             context.control.name == "space"))
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void OnDestroy()
    {
        if (_buttonAction != null)
        {
            _buttonAction.performed -= ButtonPressed;
            _buttonAction.Disable();
        }
    }
}
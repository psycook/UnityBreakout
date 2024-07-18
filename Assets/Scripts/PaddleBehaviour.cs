using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleBehaviour : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;

    private float _direction;
    private PlayerInput _playerInput;
    private InputAction _moveAction;

    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions["Move"];
        if (_moveAction != null)
        {
            _moveAction.Enable();
            _moveAction.performed += OnMovePerformed;
            _moveAction.canceled += OnMoveCancelled;
        }
    }

    private void OnDisable()
    {
        if (_moveAction != null)
        {
            _moveAction.Disable();
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        _direction = context.ReadValue<float>() * 1.5f;
    }

    private void OnMoveCancelled(InputAction.CallbackContext context)
    {
        _direction = 0.0f;
    }

    // Update is called once per frame
    void Update()
    { 
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(_direction * speed * Time.deltaTime, 0.0f, 0.0f);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -7.274f, 7.274f), transform.position.y, 0.0f);
    }
}
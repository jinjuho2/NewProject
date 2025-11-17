using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    private PlayerInputActions _input;
    private Rigidbody _rb;
    public Vector3 MoveInput { get; private set; }

    Player owner;

    private bool _isInit = false;


    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (_isInit) return;
        _input = new PlayerInputActions();
        _rb = GetComponent<Rigidbody>();
        owner = GetComponent<Player>();

        _isInit = true;
    }

    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Move.performed += OnMove;
        _input.Player.Move.canceled += OnMove;
    }

    private void OnDisable()
    {
        _input.Player.Move.performed -= OnMove;
        _input.Player.Move.canceled -= OnMove;
        _input.Disable();
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        Vector3 input = ctx.ReadValue<Vector2>();
        MoveInput = new Vector3(input.x, 0 ,input.y);
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {

    }

    private void FixedUpdate()
    {
        if (!_isInit) return;
        _rb.linearVelocity = MoveInput * owner.PlayerData.speed ; // 물리 이동
    }

}

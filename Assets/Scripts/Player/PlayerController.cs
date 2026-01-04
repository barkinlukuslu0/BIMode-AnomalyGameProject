using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private HeadBobController headBobController;
    [SerializeField] private SoundsController soundsController;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 7f;
    [SerializeField] private float runSpeed = 12f;
    [SerializeField] private float groundDrag = 5f;

    [Header("Keybinds")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Control Settings")]
    public bool canMove = true;

    private float _horizontalInput;
    private float _verticalInput;  
    private float _currentSpeed;

    Vector3 _moveDirection;
    Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true;

        _currentSpeed = walkSpeed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Inputs();
        SpeedController();
        StateHandler();
        HandleFootstepSounds();

        _rigidbody.linearDamping = groundDrag;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            MovePlayer();
        }
    }

    private void Inputs()
    {
        if (!canMove)
        {
            _horizontalInput = 0;
            _verticalInput = 0;
            return;
        }

        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void StateHandler()
    {
        if (!canMove) return;

        if (Input.GetKey(sprintKey) && (_horizontalInput != 0 || _verticalInput != 0))
        {
            _currentSpeed = runSpeed;
            if (headBobController != null) 
            {
                headBobController.ChangeAmountOfBob(20f, 0.01f);
            }
        }
        else
        {
            _currentSpeed = walkSpeed;
            if (headBobController != null) 
            {
                headBobController.ChangeAmountOfBob(10f, 0.005f);
            }
        }
    }

    private void MovePlayer()
    {
        _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;

        if (_moveDirection.magnitude > 0.1f)
        {
            _rigidbody.AddForce(_moveDirection.normalized * _currentSpeed * 10f, ForceMode.Force);
        }
    }

    private void SpeedController()
    {
        Vector3 flatVel = new Vector3(_rigidbody.linearVelocity.x, 0f, _rigidbody.linearVelocity.z);

        if (flatVel.magnitude > _currentSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * _currentSpeed;
            _rigidbody.linearVelocity = new Vector3(limitedVel.x, _rigidbody.linearVelocity.y, limitedVel.z);
        }
    }

    private void HandleFootstepSounds()
    {
        if (soundsController == null) return;

        bool isMoving = _horizontalInput != 0 || _verticalInput != 0;

        if (!canMove) isMoving = false;

        bool isRunning = Input.GetKey(sprintKey) && isMoving;

        soundsController.UpdateFootsteps(isMoving, isRunning);
    }

    public Vector3 GetVelocity()
    {
        return _rigidbody.linearVelocity;
    }
}
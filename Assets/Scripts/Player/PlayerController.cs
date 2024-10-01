using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Input")]
    [SerializeField] int playerIndex;

    [Header("Player Settings")]
    [SerializeField] float playerSpeed = 5f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float turnSmoothTime = 0.05f;
    [SerializeField] float jumpHeight = 1f;
    [SerializeField] float gravity = -9.81f;

    [Header("Grounded Check")]
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundDistance = 0.4f;
    [SerializeField] LayerMask _groundMask;


    #region private variables

    // General variables
    private CharacterController _charController;

    // Movement variables
    private float _currVelocity;
    private Vector3 _moveDirection = Vector3.zero;
    private Vector2 _inputVector = Vector2.zero;
    private bool _isGrounded;
    private Vector3 _velocity;

    #endregion

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move();
    }

    public int GetPlayerIndex()
    {
        return playerIndex;
    }

    private void Move()
    {
        // Checking if player is grounded so we can reset velocity
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
        // Adding gravity
        _velocity.y += gravity * Time.deltaTime;
        _charController.Move(_velocity * Time.deltaTime);


        // If no input, don't change anything
        if (_inputVector == Vector2.zero)
            return;

        // Moving character
        _moveDirection = new Vector3(_inputVector.x, 0f, _inputVector.y).normalized;
        _charController.Move(_moveDirection * playerSpeed * Time.deltaTime);

        // Rotating character in move direction
        float targetAngle = Mathf.Atan2(-_moveDirection.z, _moveDirection.x) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    public void Jump()
    {
        Debug.Log("attempting jump");
        if (_isGrounded)
        {
            Debug.Log("Jumping");
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void SetInputVector(CallbackContext context)
    {
        _inputVector = context.ReadValue<Vector2>();
    }
}

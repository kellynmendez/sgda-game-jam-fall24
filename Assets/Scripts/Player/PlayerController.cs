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

    #region private variables

    // General variables
    private CharacterController _charController;

    // Movement variables
    private float _currVelocity;
    private Vector3 _moveDirection = Vector3.zero;
    private Vector2 _inputVector = Vector2.zero;

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

    public void SetInputVector(CallbackContext context)
    {
        _inputVector = context.ReadValue<Vector2>();
    }
}

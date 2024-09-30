using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerSpeed = 5f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float turnSmoothTime = 0.05f;

    private CharacterController _charController;
    private float _currVelocity;
    private Vector2 _input;

    private void Awake()
    {
        _charController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal == 0 && vertical == 0)
            return;

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        _charController.Move(direction * playerSpeed * Time.deltaTime);

        float targetAngle = Mathf.Atan2(-direction.z, direction.x) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }
}

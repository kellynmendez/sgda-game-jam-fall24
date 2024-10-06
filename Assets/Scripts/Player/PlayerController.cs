using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Input")]
    [SerializeField] int playerIndex;

    [Header("Player Settings")]
    [SerializeField] float startPlayerSpeed = 15f;
    [SerializeField] float speedScaledDecr = 0.1f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] float turnSmoothTime = 0.05f;
    [SerializeField] float jumpHeight = 1f;
    [SerializeField] float gravity = -9.81f;

    [Header("Shoot Settings")]
    [SerializeField] float shootForce = 25f;
    [SerializeField] float shootUpwardForce = 10f;
    [SerializeField] float shootDelay = 0.2f;
    [SerializeField] GameObject shootOrigin;
    [SerializeField] Transform trashPlacementsParent;

    [Header("UI")]
    [SerializeField] Slider throwSlider;
    private float _throwMax = 1;

    [Header("FX")]
    [SerializeField] UnityEvent OnTrashPickUp = null;
    [SerializeField] UnityEvent OnThrow = null;
    [SerializeField] UnityEvent OnNoTrash = null;
    [SerializeField] UnityEvent OnDeath = null;

    [Header("Animation")]
    [SerializeField] Animator animator;
    private const string IDLE_ANIM = "Idle";
    private const string WALK_ANIM = "Walk";
    private const string THROW_ANIM = "Throw";

    [Header("Grounded Check")]
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundDistance = 0.4f;
    [SerializeField] LayerMask _groundMask;

    #region private variables

    // General variables
    private CharacterController _charController;
    private GameManager _gameManager;
    private bool _playerDead;
    private bool _disableInput;

    // Movement variables
    private float _playerSpeed;
    private float _currVelocity;
    private Vector3 _moveDirection = Vector3.zero;
    private Vector2 _inputVector = Vector2.zero;
    private bool _isGrounded;
    private Vector3 _velocity;
    
    // Shoot variables
    private Stack<Trash> _playerTrash;
    private List<Transform> _trashPlacements;
    private bool _held;

    // FX
    AudioSource _audioSource;

    #endregion

    private void Awake()
    {
        _playerSpeed = startPlayerSpeed;
        _charController = GetComponent<CharacterController>();
        _gameManager = FindObjectOfType<GameManager>();
        _audioSource = GetComponent<AudioSource>();
        _playerDead = false;
        _disableInput = false;

        // trash
        _held = false;
        _playerTrash = new Stack<Trash>();
        _trashPlacements = new List<Transform>();
        foreach (Transform child in trashPlacementsParent)
        {
            _trashPlacements.Add(child);
        }
    }

    private void Start()
    {
        animator.Play(IDLE_ANIM);
    }

    private void Update()
    {
        Move();

        _playerSpeed = startPlayerSpeed * (1 - (_playerTrash.Count * speedScaledDecr));
    }

    public int GetPlayerIndex()
    {
        return playerIndex;
    }

    #region Movement
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
        if (_inputVector == Vector2.zero || _playerDead || _disableInput)
        {
            animator.Play(IDLE_ANIM);
            return;
        }

        animator.Play(WALK_ANIM);

        // Moving character
        _moveDirection = new Vector3(_inputVector.x, 0f, _inputVector.y).normalized;
        _charController.Move(_moveDirection * _playerSpeed * Time.deltaTime);

        // Rotating character in move direction
        float targetAngle = Mathf.Atan2(-_moveDirection.z, _moveDirection.x) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    public void Jump()
    {
        if (_isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    #endregion

    public void SetInputVector(CallbackContext context)
    {
        _inputVector = context.ReadValue<Vector2>();
    }

    public void DisableInput()
    {
        _disableInput = true;
    }

    public bool IsDead()
    {
        return _playerDead;
    }

    public void Kill()
    {
        _playerDead = true;
        Debug.Log("player DIE");
        OnDeath?.Invoke();
        _gameManager.DisableAllPlayerInput();
        StartCoroutine(EndGame());
    }

    #region Shooting
    public void AddTrashToPlayer(Trash trash)
    {
        OnTrashPickUp?.Invoke();
        _playerTrash.Push(trash);
    }

    public int GetTrashNumber()
    {
        return _playerTrash.Count;
    }

    public void SetIsHeld(bool held)
    {
        _held = held;
        if (_held)
        {
            StartCoroutine(FullShootRoutine(Shoot));
        }
    }

    private void Shoot()
    {
        if (_playerTrash.Count > 0)
        {
            animator.Play(THROW_ANIM);
            OnThrow?.Invoke();
            // Getting latest trash
            GameObject trashToShoot = _playerTrash.Pop().gameObject;
            // Setting starting location to shoot origin and activating
            trashToShoot.transform.position = shootOrigin.transform.position;
            trashToShoot.transform.forward = shootOrigin.transform.forward;
            // Setting trash's shoot settings
            Trash trash = trashToShoot.GetComponent<Trash>();
            trash.SetAttachedToPlayer(false);
            trashToShoot.transform.parent = null;
            // Shoot it
            Rigidbody rb = trashToShoot.GetComponent<Rigidbody>();
            if (rb)
            {
                trash.UnfreezePosition();
                Vector3 forceToAdd = shootForce * trashToShoot.transform.forward.normalized
                    + trashToShoot.transform.up * shootUpwardForce;
                rb.AddForce(forceToAdd, ForceMode.Impulse);
            }
        }
    }

    public List<Transform> GetTrashPlacements()
    {
        return _trashPlacements;
    }
    #endregion

    #region FX
    public void PlayFX(AudioClip sfx)
    {
        _audioSource.PlayOneShot(sfx, _audioSource.volume);
    }
    #endregion

    #region Coroutines
    private IEnumerator FullShootRoutine(System.Action ShootFn)
    {
        _disableInput = true;
        while (_held)
        {
            throwSlider.value = 0;
            float elapsed = 0f;
            while (elapsed < shootDelay)
            {
                throwSlider.value += Time.deltaTime;
                elapsed += Time.deltaTime;
            }
            throwSlider.value = 1;

            yield return new WaitForSecondsRealtime(shootDelay);
            if (_held)
            {
                if (_playerTrash.Count > 0)
                {
                    ShootFn();
                }
                else
                {
                    OnNoTrash?.Invoke();
                }
            }
        }
        yield return null;
        _disableInput = false;
    }
    
    private IEnumerator EndGame()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1;
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
    #endregion
}

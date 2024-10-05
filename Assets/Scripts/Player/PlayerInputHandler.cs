using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput _playerInput;
    private PlayerController _playerController;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        int index = _playerInput.playerIndex;
        PlayerController[] playerCtrlrs = FindObjectsOfType<PlayerController>();
        _playerController = playerCtrlrs.FirstOrDefault(m => m.GetPlayerIndex() == index);
    }

    public void SetPlayerMoveInput(CallbackContext context)
    {
        _playerController.SetInputVector(context);
    }

    public void SetPlayerShootInput(CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                _playerController.SetIsHeld(true);
                break;
            case InputActionPhase.Canceled:
                _playerController.SetIsHeld(false);
                break;
        }
    }

    public void SetPlayerJumpInput(CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Performed:
                //_playerController.Jump();
                break;
                //case InputActionPhase.Started:
                //    Debug.Log("Started");
                //    break;
                //case InputActionPhase.Canceled:
                //    Debug.Log("Canceled");
                //    break;
        }
    }
}

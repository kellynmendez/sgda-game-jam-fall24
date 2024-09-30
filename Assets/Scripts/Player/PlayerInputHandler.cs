using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
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
        Debug.Log(index);
        //if (index == 1)
        //{
        //    _playerInput.SwitchCurrentActionMap("MovementKeyboardRight");
        //}
    }

    public void SetPlayerMoveInput(CallbackContext context)
    {
        _playerController.SetInputVector(context);
    }
}

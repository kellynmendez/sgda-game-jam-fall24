using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] GameObject playerInputPrefab;
    private PlayerInput _player1;
    private PlayerInput _player2;

    private void Start()
    {
        _player1 = PlayerInput.Instantiate(playerInputPrefab, pairWithDevice: Keyboard.current);
        _player2 = PlayerInput.Instantiate(playerInputPrefab, pairWithDevice: Keyboard.current);

        _player1.SwitchCurrentActionMap("MovementKeyboardLeft");
        _player2.SwitchCurrentActionMap("MovementKeyboardRight");
    }
}

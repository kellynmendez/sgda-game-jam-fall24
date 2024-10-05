using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerController[] _players;
    private void Awake()
    {
        _players = FindObjectsOfType<PlayerController>();
    }

    public void DisableAllPlayerInput()
    {
        foreach (PlayerController player in _players)
        {
            player.DisableInput();
        }
    }
}

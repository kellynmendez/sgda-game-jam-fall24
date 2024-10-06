using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillVolume : MonoBehaviour
{
    private bool _loserAssigned;

    private void Awake()
    {
        _loserAssigned = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (!player.IsDead())
            {
                if (!_loserAssigned)
                {
                    PlayerPrefs.SetInt("PlayerDied", player.GetPlayerIndex());
                    Debug.Log(player.GetPlayerIndex() + " LOST");
                    _loserAssigned = true;
                }
                player.Kill();
            }
        }
    }
}

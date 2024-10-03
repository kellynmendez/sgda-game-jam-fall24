using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

[RequireComponent(typeof(Rigidbody))]
public class Trash : MonoBehaviour
{
    [SerializeField] Collider _triggerToDisable;
    [SerializeField] GameObject _artToDisable;

    private bool _attachedToPlayer = false;

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.CompareTag("KillVolume"))
        //{
        //    Destroy(gameObject);
        //    return;
        //}


        // If the player is the one shooting this, don't trigger
        //   otherwise, allow player to pick up the trash
        PlayerShoot player = other.gameObject.GetComponent<PlayerShoot>();
        if(!_attachedToPlayer && player != null)
        {
            // Trash was picked up, disable trash
            Debug.Log("added to player");
            player.AddTrashToPlayer(this);
            Deactivate();
            _attachedToPlayer = true;
            this.gameObject.SetActive(false);
            return;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        PlayerShoot player = other.gameObject.GetComponent<PlayerShoot>();
        if (player != null)
        {
            Debug.Log("removed from player");
            _attachedToPlayer = false;
            return;
        }
    }

    public void Deactivate()
    {
        //_triggerToDisable.enabled = false;
        _artToDisable.SetActive(false);
    }
    public void Activate()
    {
        //_triggerToDisable.enabled = true;
        _artToDisable.SetActive(true);
    }
}

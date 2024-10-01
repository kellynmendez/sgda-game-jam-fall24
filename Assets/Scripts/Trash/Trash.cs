using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Trash : MonoBehaviour
{
    [SerializeField] GameObject _artToDisable;

    private Collider _triggerToDisable;
    private HashSet<GameObject> _objsNotToTrigger;

    private void Awake()
    {
        _triggerToDisable = GetComponent<BoxCollider>();
        _objsNotToTrigger = new HashSet<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Floor"))
        {
            _objsNotToTrigger.Clear();
            return;
        }

        if (other.gameObject.CompareTag("KillVolume"))
        {
            _objsNotToTrigger.Clear();
            Destroy(gameObject);
            return;
        }


        // If the player is the one shooting this, don't trigger
        //   otherwise, allow player to pick up the trash
        PlayerShoot player = other.gameObject.GetComponent<PlayerShoot>();
        if(!_objsNotToTrigger.Contains(other.gameObject) && player != null)
        {
            // Trash was picked up, disable trash
            player.AddTrashToPlayer(this);
            _objsNotToTrigger.Clear();
            _objsNotToTrigger.Add(other.gameObject);
            Deactivate();
            return;
        }

    }

    private void Update()
    {

    }

    public void ClearThenAddToCollidersNotToTrigger(Collider collider)
    {
        _objsNotToTrigger.Clear();
        _objsNotToTrigger.Add(collider.gameObject);
    }

    public void Deactivate()
    {
        _triggerToDisable.enabled = false;
        _artToDisable.SetActive(false);
    }
    public void Activate()
    {
        _triggerToDisable.enabled = true;
        _artToDisable.SetActive(true);
    }
}

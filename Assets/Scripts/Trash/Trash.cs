using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Trash : MonoBehaviour
{
    [SerializeField] Collider _triggerToDisable;
    [SerializeField] Collider _solidColliderToDisable;

    private bool _attachedToPlayer = false;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("KillVolume"))
        {
            Kill();
            this.gameObject.transform.parent = null;
            return;
        }

        // If the player is the one shooting this, don't trigger
        //   otherwise, allow player to pick up the trash
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if(!_attachedToPlayer && player != null)
        {
            _attachedToPlayer = true;
            // Trash was picked up, disable trash
            player.AddTrashToPlayer(this);
            Vector3 pointOfColl = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            List<Transform> places = player.GetTrashPlacements();
            // Find first transform with no children
            Transform finalPlace = places[0];
            //foreach (Transform t in places)
            //{
            //    if (t.childCount == 0)
            //    {
            //        finalPlace = t;
            //        return;
            //    }
            //}
            // Find closest transform in list with no children
            float smallestDist = Vector3.Distance(pointOfColl, finalPlace.position);
            foreach (Transform t in places)
            {
                float checkDist = Vector3.Distance(pointOfColl, t.position);
                if (t.childCount == 0 && checkDist < smallestDist)
                {
                    smallestDist = checkDist;
                    finalPlace = t;
                }
            }
            this.gameObject.transform.parent = finalPlace;
            FreezePosition();
            this.gameObject.transform.position = finalPlace.position;
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !_attachedToPlayer)
        {
            UnfreezePosition();
            _attachedToPlayer = false;
            this.transform.parent = null;
        }
    }

    public void Deactivate()
    {
        _solidColliderToDisable.enabled = false;
    }

    public void Activate()
    {
        _solidColliderToDisable.enabled = true;
    }

    public void Kill()
    {
        Destroy(this.gameObject);
    }

    public void FreezePosition()
    {
        _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void UnfreezePosition()
    {
        _rigidbody.constraints = RigidbodyConstraints.None;
    }

    public void SetAttachedToPlayer(bool att)
    {
        _attachedToPlayer = att;
    }
}

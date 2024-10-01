using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Trash Physics")]
    [SerializeField] float shootForce = 1f;
    [SerializeField] float shootUpwardForce = 1f;

    [Header("Miscellaneous")]
    [SerializeField] GameObject shootOrigin;

    private Stack<Trash> _playerTrash;

    private void Awake()
    {
        _playerTrash = new Stack<Trash>();
    }

    public void AddTrashToPlayer(Trash trash)
    {
        _playerTrash.Push(trash);
    }

    public void Shoot()
    {
        if (_playerTrash.Count > 0)
        {
            // Getting latest trash
            GameObject trashToShoot = _playerTrash.Pop().gameObject;
            // Setting starting location to shoot origin and activating
            trashToShoot.transform.position = shootOrigin.transform.position;
            trashToShoot.transform.forward = shootOrigin.transform.forward;
            trashToShoot.GetComponent<Trash>().Activate();
            // Shoot it
            Rigidbody rb = trashToShoot.GetComponent<Rigidbody>();
            if (rb)
            {
                Vector3 forceToAdd = shootForce * trashToShoot.transform.forward + trashToShoot.transform.up * shootUpwardForce;
                rb.AddForce(forceToAdd, ForceMode.Impulse);
            }
        }
        else
        {
            Debug.Log("Can't shoot!");
        }
    }
}

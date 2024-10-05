using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider))]
public class TrashPool : MonoBehaviour
{
    [SerializeField] float spawnDelay = 0f;
    [SerializeField] float spawnRate = 3f;
    [SerializeField] List<GameObject> _trashPrefabs = new List<GameObject>();

    private Collider _areaOfSpawn;

    private void Awake()
    {
        _areaOfSpawn = GetComponent<Collider>();
    }

    private void Start()
    {
        InvokeRepeating("SpawnTrash", spawnDelay, spawnRate);
    }

    private void SpawnTrash()
    {
        GameObject toSpawn;
        int rand = Random.Range(0, _trashPrefabs.Count);
        toSpawn = Instantiate(_trashPrefabs[rand]);
        toSpawn.transform.position = RandomPointInBounds(_areaOfSpawn.bounds);
        toSpawn.transform.rotation = Random.rotation;
    }

    private static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}

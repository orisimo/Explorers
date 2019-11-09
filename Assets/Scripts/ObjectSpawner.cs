using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private List<ObjectQuantityItem> _objectQuantitiesList = new List<ObjectQuantityItem>();

    private void Awake()
    {
        foreach (var objectQuantityItem in _objectQuantitiesList)
        {
            for (int spawnCountIndex = 0; spawnCountIndex < objectQuantityItem.SpawnCount; spawnCountIndex++)
            {
                var randomPosition = new Vector3(
                    UnityEngine.Random.Range(-objectQuantityItem.SpawnArea.x, objectQuantityItem.SpawnArea.x),
                    UnityEngine.Random.Range(-objectQuantityItem.SpawnArea.y, objectQuantityItem.SpawnArea.y),
                    UnityEngine.Random.Range(-objectQuantityItem.SpawnArea.z, objectQuantityItem.SpawnArea.z)
                );
                Instantiate(objectQuantityItem.SpawnObject, randomPosition, Quaternion.Euler(objectQuantityItem.EulerRotation));
            }
        }
    }
}

[Serializable]
public struct ObjectQuantityItem
{
    public GameObject SpawnObject;
    public int SpawnCount;
    public Vector3 SpawnArea;
    public Vector3 EulerRotation;
}

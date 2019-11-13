using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using Random = System.Random;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner Instance;
    private static Collider[] _overlapResults = new Collider[1];
    private const int MAX_SPAWN_RETRIES = 5;
    private const float OVERLAP_RADIUS = 3f;
    private int _groundlessLayerMask;
    
    [SerializeField] private PrefabDictionary _prefabDictionary = new PrefabDictionary();
    [SerializeField] private List<ObjectSpawnData> _objectQuantitiesList = new List<ObjectSpawnData>();
    
    private void Awake()
    {
        Instance = this;
        _groundlessLayerMask = ~ (1 << LayerMask.NameToLayer("Ground"));
        
        foreach (var objectQuantityItem in _objectQuantitiesList)
        {
            for (int spawnCountIndex = 0; spawnCountIndex < objectQuantityItem.SpawnCount; spawnCountIndex++)
            {
                var randomPosition = new Vector3(
                    UnityEngine.Random.Range(-objectQuantityItem.SpawnArea.x, objectQuantityItem.SpawnArea.x),
                    UnityEngine.Random.Range(-objectQuantityItem.SpawnArea.y, objectQuantityItem.SpawnArea.y),
                    UnityEngine.Random.Range(-objectQuantityItem.SpawnArea.z, objectQuantityItem.SpawnArea.z)
                );
                randomPosition += objectQuantityItem.SpawnPosition;
                for (int i = 0; i < MAX_SPAWN_RETRIES; i++)
                {
                    if (SpawnItemByType(objectQuantityItem.SpawnType, randomPosition, Quaternion.Euler(objectQuantityItem.EulerRotation), null, false))
                    {
                        break;
                    }
                }
            }
        }
    }

    public bool SpawnItemByType(ItemType typeToSpawn, Vector3 spawnPosition, Quaternion spawnRotation = default(Quaternion), Transform parent = null, bool ignoreRaycast = true)
    {
        if(!_prefabDictionary.TryGetValue(typeToSpawn, out var prefab))
        {
            return false;
        }

        _overlapResults[0] = null;
        if(!ignoreRaycast)
        {
            Physics.OverlapSphereNonAlloc(spawnPosition, OVERLAP_RADIUS, _overlapResults, _groundlessLayerMask);
            if (_overlapResults[0])
            {
                return false;
            }
        }
        Instantiate(prefab, spawnPosition, spawnRotation, parent);
        return true;
    }
}
[Serializable]
public class PrefabDictionary : SerializableDictionaryBase<ItemType, GameObject> { }
[Serializable]
public struct ObjectSpawnData
{
    public ItemType SpawnType;
    public int SpawnCount;
    public Vector3 SpawnPosition;
    public Vector3 SpawnArea;
    public Vector3 EulerRotation;
}

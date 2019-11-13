using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using Utils;

public class BaseHarvestable : MonoBehaviour, IHarvestable
{
    [SerializeField] private ItemType _yieldType;
    [SerializeField] private float[] _yieldPercents;
    [SerializeField] private float _yieldSpawnRadius = 1f;
    [SerializeField] private int _totalHarvestPoints = 100;
    [SerializeField] private bool _destroyOnHarvestComplete;
    private int _currentHarvestPoints;
    private int _currentYieldPercentIndex;
    
    public float PercentHarvested => (float) _currentHarvestPoints / _totalHarvestPoints;
    
    public virtual void HarvestTick(int harvestPoints)
    {
        _currentHarvestPoints -= harvestPoints;
        
        while (_currentYieldPercentIndex < _yieldPercents.Length && 
               PercentHarvested < _yieldPercents[_currentYieldPercentIndex])
        {
            Yield();
            _currentYieldPercentIndex++;
            if (_currentYieldPercentIndex > _yieldPercents.Length)
            {
                break;
            }
        }

        if (_currentHarvestPoints >= 0)
        {
            return;
        }
        
        OnHarvestComplete();
    }

    protected virtual void Awake()
    {
        _currentHarvestPoints = _totalHarvestPoints;
    }

    protected virtual void Yield()
    {
        var randomSpawnPosition = MathUtil.GetPointAroundHorizontalRadius(transform.position, _yieldSpawnRadius);
        ObjectSpawner.Instance.SpawnItemByType(_yieldType, randomSpawnPosition);
    }

    protected virtual void OnHarvestComplete()
    {
        if (_destroyOnHarvestComplete)
        {
            Destroy(gameObject);
        }
    }
}
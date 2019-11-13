using System;
using System.Collections.Generic;
using Items;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;
using Utils;

public class Blueprint : BaseContainer, IConsumer
{
    [SerializeField] private PricesDictionary _price = new PricesDictionary();
    [SerializeField] private ItemType _resultItem;
    [SerializeField] private float _resultYieldRadius;
    [SerializeField] private bool _destroyOnPaymentsFulfilled = false;
    
    public PricesDictionary Price => _price;

    private PricesDictionary _remainingPrice;

    private void Awake()
    {
        ResetPrice();
    }
    
    private void ResetPrice()
    {
        _remainingPrice = _price;
    }
    
    public override bool TryDepositItem(IDepositable itemToDeposit)
    {
        return Price.ContainsKey(itemToDeposit.Type) && 
               base.TryDepositItem(itemToDeposit);
    }
    
    public override bool TryWithdrawItem(IDepositable depositableToWithdraw, Vector3? withdrawPosition = null)
    {
        return false;
    }
    
    public void Pay(ItemType currency)
    {
        _price[currency] -= 1;
        if (_price[currency] <= 0)
        {
            _price.Remove(currency);
        }
        
        if (_price.Count > 0)
        {
            return;
        }
        
        var spawnPosition = MathUtil.GetPointAroundHorizontalRadius(transform.position, _resultYieldRadius);
        ObjectSpawner.Instance.SpawnItemByType(_resultItem, spawnPosition, transform.rotation);
        if (_destroyOnPaymentsFulfilled)
        {
            Destroy(gameObject);
        }
        else
        {
            ResetPrice();
        }
    }
}

[Serializable]
public class PricesDictionary : SerializableDictionaryBase<ItemType, int>
{
}
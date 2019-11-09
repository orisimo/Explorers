using System;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class Campfire : Building, IContainer
{
    [SerializeField] private int _slotCount;
    [SerializeField] private Transform[] _itemSlotPivots;
    [SerializeField] private Image[] _slotCookedPercentProgressBars;

    private List<Coroutine> _pivotSnapCoroutines = new List<Coroutine>();
    
    public List<IDepositable> DepositedItems { get; set; } = new List<IDepositable>();
    
    public bool TryDepositeItem(IDepositable itemToDeposite)
    {
        var foodItem = itemToDeposite as FoodItem;
        
        if (foodItem == null)
        {
            return false;
        }
        
        if (DepositedItems.Count >= _slotCount)
        {
            return false;
        }

        foodItem.Deposit(this);
        DepositedItems.Add(foodItem);
        var slotIndex = DepositedItems.IndexOf(foodItem);
        var snapCoroutine =
            StartCoroutine(TweenUtil.EaseTransformToPoint(foodItem.Transform, _itemSlotPivots[slotIndex]));
        _pivotSnapCoroutines.Add(snapCoroutine);

        return true;
    }

    public bool TryWithdrawItem(IDepositable depositableToWithdraw)
    {
        if (depositableToWithdraw == null)
        {
            return false;
        }
        var slotIndex = DepositedItems.IndexOf(depositableToWithdraw);
        var snapCoroutine = _pivotSnapCoroutines[slotIndex];
        StopCoroutine(snapCoroutine);
        _pivotSnapCoroutines.Remove(snapCoroutine);

        depositableToWithdraw.Withdraw();
        DepositedItems.Remove(depositableToWithdraw);
        return true;
    }

    private void Update()
    {
        for (var itemSlotIndex = 0; itemSlotIndex < DepositedItems.Count; itemSlotIndex++)
        {
            var foodItem = DepositedItems[itemSlotIndex];
            var cookableItem = foodItem as ICookable;
            if (cookableItem == null) continue;
            cookableItem.CookedPercent += Time.deltaTime / cookableItem.CookDuration;
            _slotCookedPercentProgressBars[itemSlotIndex].fillAmount = cookableItem.CookedPercent;
        }
    }
}

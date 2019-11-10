using System;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class Campfire : Building, IContainer, IDepositable
{
    [SerializeField] private int _slotCount;
    [SerializeField] private ContainerSlot[] _containerSlots;
    [SerializeField] private Image[] _slotCookedPercentProgressBars;

    public bool HasEmptySlots => DepositedItems.Count < _slotCount;
    public List<IDepositable> DepositedItems { get; set; } = new List<IDepositable>();
    
    public void DepositeItem(IDepositable itemToDeposite)
    {
        var foodItem = itemToDeposite as FoodItem;
        
        if (foodItem == null)
        {
            return;
        }

        foodItem.Deposit(this);
        DepositedItems.Add(foodItem);
        var containerSlot = GetEmptySlot();
        foodItem.Rigidbody.isKinematic = true;
        foodItem.Transform.position = containerSlot.Transform.position;
        foodItem.Transform.SetParent(containerSlot.Transform);
        containerSlot.IsEmpty = false;
    }

    private ContainerSlot GetEmptySlot()
    {
        for (var slotIndex = 0; slotIndex < _containerSlots.Length; slotIndex++)
        {
            var containerSlot = _containerSlots[slotIndex];
            if (!containerSlot.IsEmpty)
            {
                continue;
            }

            return containerSlot;
        }

        return null;
    }

    private ContainerSlot GetGrabbableSlot(IGrabbable grabbable)
    {
        for (var slotIndex = 0; slotIndex < _containerSlots.Length; slotIndex++)
        {
            var containerSlot = _containerSlots[slotIndex];
            if (!containerSlot.Transform == grabbable.Transform)
            {
                continue;
            }

            return containerSlot;
        }

        return null;
    }
    
    
    public bool TryWithdrawItem(IDepositable depositableToWithdraw, Vector3? withdrawPosition)
    {
        if (depositableToWithdraw == null)
        {
            return false;
        }

        var grabbable = depositableToWithdraw as IGrabbable;
        var containerSlot = GetGrabbableSlot(grabbable);
        
        depositableToWithdraw.Withdraw();
        DepositedItems.Remove(depositableToWithdraw);
        
        grabbable.Rigidbody.isKinematic = true;
        grabbable.Transform.position = containerSlot.Transform.position;
        grabbable.Transform.SetParent(containerSlot.Transform);
        
        containerSlot.IsEmpty = true;
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

    public bool IsDeposited { get; set; }
    public void Deposit(IContainer container)
    {
        IsDeposited = true;
        Container = container;
    }

    public void Withdraw()
    {
        IsDeposited = false;
        Container = null;
    }

    public IContainer Container { get; set; }
}

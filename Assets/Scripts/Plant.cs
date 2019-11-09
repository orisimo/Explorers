using System;
using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using Random = System.Random;

public class Plant : MonoBehaviour, IContainer
{
    [SerializeField] private Transform[] _branches;
    [SerializeField] private FoodItem[] _yieldPrefabs;
    public List<IDepositable> DepositedItems { get; set; } = new List<IDepositable>();
    
    private FoodItem[] _foodItems;
    
    public bool TryDepositeItem(IDepositable itemsToDeposite)
    {
        return false;
    }

    public bool TryWithdrawItem(IDepositable depositableToWithdraw)
    {
        depositableToWithdraw.Withdraw();
        DepositedItems.Remove(depositableToWithdraw);
        var grabbable = depositableToWithdraw as GrabbableItem;
        grabbable.Rigidbody.isKinematic = false;
        RemoveFoodItem(depositableToWithdraw);
        return true;
    }

    private void Awake()
    {
        InitializeFruit();
    }

    private void InitializeFruit()
    {
        _foodItems = new FoodItem[_branches.Length];
        for (var foodItemIndex = 0; foodItemIndex < _foodItems.Length; foodItemIndex++)
        {
            InstantiateRandomFruitAtIndex(foodItemIndex);
        }
    }

    private void InstantiateRandomFruitAtIndex(int foodItemIndex)
    {
        var randomFruitPrefabIndex = UnityEngine.Random.Range(0, _yieldPrefabs.Length);
        var randomFruitPrefab = _yieldPrefabs[randomFruitPrefabIndex];
        var branch = _branches[foodItemIndex];
        var foodItem = _foodItems[foodItemIndex] = Instantiate(randomFruitPrefab, branch, false);
        foodItem.Transform.localPosition = Vector3.zero;
        foodItem.Transform.localRotation = UnityEngine.Random.rotation;
        foodItem.Rigidbody.isKinematic = true;
        foodItem.Deposit(this);
        DepositedItems.Add(foodItem);
    }

    private void Update()
    {
        TickYield();
    }

    private void RemoveFoodItem(IDepositable depositable)
    {
        var foodItem = depositable as FoodItem;
        if (foodItem == null)
        {
            Debug.LogErrorFormat("Couldn't find the requested depositable in this plant's food items");
            return;
        }

        for (var branchIndex = 0; branchIndex < _branches.Length; branchIndex++)
        {
            if (foodItem == _foodItems[branchIndex])
            {
                DepositedItems.Remove(depositable);
                _foodItems[branchIndex] = null;
            }
        }
    }
    
    private void TickYield()
    {
        for (var branchIndex = 0; branchIndex < _branches.Length; branchIndex++)
        {
            var foodItem = _foodItems[branchIndex];
            if (foodItem == null)
            {
                InstantiateRandomFruitAtIndex(branchIndex);
                continue;
            }
            foodItem.RipePercent += Time.deltaTime / foodItem.RipenDuration;
        }
    }
}

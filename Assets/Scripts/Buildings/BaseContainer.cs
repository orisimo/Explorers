using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using Utils;

public class BaseContainer : MonoBehaviour, IContainer
{
    [SerializeField] private Transform[] _storageSlots;
    public List<IDepositable> DepositedItems { get; set; } = new List<IDepositable>();
    public bool HasEmptySlots => DepositedItems.Count < _storageSlots.Length;
    
    public virtual bool TryDepositItem(IDepositable itemToDeposite)
    {
        var itemIndex = DepositedItems.Count;
        var grabbable = itemToDeposite as IGrabbable;
        var slot = _storageSlots[itemIndex];
        grabbable.Collider.enabled = false;
        grabbable.Transform.position = slot.position;
        grabbable.Rigidbody.velocity = Vector3.zero;
        grabbable.Rigidbody.isKinematic = true;
        grabbable.Transform.SetParent(slot);
        DepositedItems.Add(itemToDeposite);
        return true;
    }

    public virtual bool TryWithdrawItem(IDepositable depositableToWithdraw, Vector3? withdrawPosition = null)
    {
        if (!DepositedItems.Contains(depositableToWithdraw))
        {
            return false;
        }
        var grabbable = depositableToWithdraw as IGrabbable;
        grabbable.Collider.enabled = true;
        grabbable.Rigidbody.isKinematic = false;
        grabbable.Transform.SetParent(null);

        if (withdrawPosition.HasValue)
        {
            grabbable.Transform.position = withdrawPosition.Value;
        }
        return DepositedItems.Remove(depositableToWithdraw);
    }

    public bool TryWithdrawFirstItem(out IDepositable withdrawnItem, Vector3? withdrawPosition = null)
    {
        if (DepositedItems.Count <= 0)
        {
            withdrawnItem = null;
            return false;
        }
        withdrawnItem = DepositedItems[0];
        if (TryWithdrawItem(withdrawnItem, withdrawPosition))
        {
            return true;
        }
        withdrawnItem = null;
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
using Utils;

public class Cart : MonoBehaviour, IContainer
{
    [SerializeField] private Transform[] _storageSlots;
    public List<IDepositable> DepositedItems { get; set; } = new List<IDepositable>();
    public bool HasEmptySlots => DepositedItems.Count < _storageSlots.Length;

    public void DepositeItem(IDepositable itemToDeposite)
    {
        var itemIndex = DepositedItems.Count;
        var grabbable = itemToDeposite as IGrabbable;
        var slot = _storageSlots[itemIndex];
        grabbable.Transform.position = slot.position;
        grabbable.Rigidbody.velocity = Vector3.zero;
        grabbable.Rigidbody.isKinematic = true;
        grabbable.Transform.SetParent(slot);
        DepositedItems.Add(itemToDeposite);
    }

    public bool TryWithdrawItem(IDepositable depositableToWithdraw, Vector3? withdrawPosition = null)
    {
        if (!DepositedItems.Contains(depositableToWithdraw))
        {
            return false;
        }
        var grabbable = depositableToWithdraw as IGrabbable;
        grabbable.Rigidbody.isKinematic = false;
        grabbable.Transform.SetParent(null);

        if (withdrawPosition.HasValue)
        {
            grabbable.Transform.position = withdrawPosition.Value;
        }
        return DepositedItems.Remove(depositableToWithdraw);
    }
}

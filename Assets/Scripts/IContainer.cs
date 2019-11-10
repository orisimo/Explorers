using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

public interface IContainer
{
    List<IDepositable> DepositedItems { get; set; }
    void DepositeItem(IDepositable itemToDeposite);
    bool HasEmptySlots { get; }
    bool TryWithdrawItem(IDepositable depositableToWithdraw, Vector3? withdrawPosition = null);
}

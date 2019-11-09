using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

public interface IContainer
{
    List<IDepositable> DepositedItems { get; set; }
    bool TryDepositeItem(IDepositable itemsToDeposite);
    bool TryWithdrawItem(IDepositable depositableToWithdraw);
}

using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

public class BaseDepositable : BaseGrabbable, IDepositable
{
    [SerializeField] private ItemType _type;
    public ItemType Type => _type;
    public bool IsDeposited { get; set; }
    public virtual void Deposit(IContainer container)
    {
        IsDeposited = true;
        Container = container;
    }

    public virtual void Withdraw()
    {
        IsDeposited = false;
        Container = null;
    }

    public IContainer Container { get; set; }
}

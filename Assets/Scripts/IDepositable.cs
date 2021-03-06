﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDepositable
{
    ItemType Type { get; }
    bool IsDeposited { get; set; }
    void Deposit(IContainer container);
    void Withdraw();
    IContainer Container { get; set; }
}

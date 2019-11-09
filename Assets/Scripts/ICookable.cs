using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICookable
{
    float CookDuration { get;}
    float CookedPercent { get; set; }
    bool IsCooked { get; }
}

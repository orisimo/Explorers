using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRipenable
{
    float RipenDuration { get; }
    float RipePercent { get; set; }

    bool IsRipe { get; }
}

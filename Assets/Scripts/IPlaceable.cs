using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlaceable
{
    Rigidbody Rigidbody { get; }
    float UnpackDuration { get; }
    bool IsGrabbed { get; set; }
    bool IsPlaced { get; }
    bool IsPacked { get; }
    float UnpackedPercent { get; set; }
    void Dismantle(Transform playerTransform);
    void Place();
}

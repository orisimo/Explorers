using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMovementController : MonoBehaviour
{
    [SerializeField] private WheelCollider[] _wheels;
    [SerializeField] private Vector2 _brakeTorqueMinMax = new Vector2(0f, 200f);

    private void Awake()
    {
        SetBreaks(true);
    }

    public void SetBreaks(bool isBreakOn)
    {
        for (var wheelIndex = 0; wheelIndex < _wheels.Length; wheelIndex++)
        {
            var wheelCollider = _wheels[wheelIndex];
            wheelCollider.brakeTorque = isBreakOn ? _brakeTorqueMinMax.y : _brakeTorqueMinMax.x;
        }
    }
}

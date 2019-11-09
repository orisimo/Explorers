using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;

public class Building : MonoBehaviour, IPlaceable, IGrabbable
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _unpackDuration;
    private float _unpackedPercent = 1f;

    public Rigidbody Rigidbody => _rigidbody;
    public Transform Transform => transform;
    public Coroutine GrabCoroutine { get; set; }

    public float UnpackDuration => _unpackDuration;
    
    public bool IsGrabbed { get; set; }
    
    public bool IsPlaced => UnpackedPercent >= 1f;
    public bool IsPacked => UnpackedPercent <= 0f;
    
    public float UnpackedPercent
    {
        get => _unpackedPercent;
        set { _unpackedPercent = value; }
    }

    public void Dismantle(Transform playerTransform)
    {
        transform.SetParent(playerTransform);
        Rigidbody.isKinematic = false;
    }

    public void Place()
    {
        transform.SetParent(null);
        
        var currentPosition = transform.position;
        currentPosition.y = 0f;
        transform.position = currentPosition;
        
        Rigidbody.isKinematic = true;
        Rigidbody.transform.rotation = Quaternion.identity;
        _unpackedPercent = 1f;
    }
}

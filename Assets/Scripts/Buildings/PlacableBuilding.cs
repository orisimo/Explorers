using System.Collections;
using System.Collections.Generic;
using Items;
using UnityEngine;
public class PlacableBuilding : MonoBehaviour, IPlaceable, IGrabbable
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;
    [SerializeField] private float _unpackDuration;
    private float _unpackedPercent = 1f;


    public Rigidbody Rigidbody => _rigidbody;
    public Collider Collider => _collider;
    public Transform Transform => transform;
    public Coroutine GrabCoroutine { get; set; }
    public void OnGrab(PlayerContext grabbingPlayerContext)
    {
        _collider.enabled = false;
    }

    public void OnRelease()
    {
        _collider.enabled = true;
    }

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
        Collider.enabled = false;
        Rigidbody.isKinematic = false;
    }

    public void Place()
    {
        transform.SetParent(null);
        
        var currentPosition = Transform.position;
        var rayCastRay = new Ray(currentPosition, Vector3.down);
        var result = new RaycastHit[1];
        Physics.RaycastNonAlloc(rayCastRay, result, 10f, ObjectSpawner.Instance.GroundLayerMask);
        currentPosition.y = result[0].point.y;
        transform.position = currentPosition;
        Collider.enabled = true;
        
        Rigidbody.isKinematic = true;
        Rigidbody.transform.rotation = Quaternion.identity;
        _unpackedPercent = 1f;
    }
}

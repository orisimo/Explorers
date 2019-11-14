using System.Collections.Generic;
using Items;
using UnityEngine;
using Utils;

[RequireComponent(typeof(PlayerContext))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerInteractionController : MonoBehaviour
{
    [Header("Interaction Parameters")]
    [SerializeField] private LayerMask _interactLayer;
    [SerializeField] private float _interactionRadius = 1f;
    [Header("Grab Parameters")]
    [SerializeField] private Transform _grabPivot;
    [SerializeField] private float _grabConeAngle = 45f;
    [SerializeField] private float _grabEaseModifier = 0.4f;
    [SerializeField] private int _grabSlotCount = 1;

    [Header("Harvest Parameters")] [SerializeField]
    private int _harvestRate = 30;

    [Header("Dismantle Parameters")] [SerializeField]
    private float _dismantleDelay = 0.5f;

    private const int OVERLAP_SPHERE_ARRAY_SIZE = 10;    
    private const float GRAB_PIVOT_SNAP_THRESHOLD = 0.1f;
    
    private PlayerContext _playerContext;
    
    private Rigidbody _playerRigidbody;
    private float _initialMass;
    private float _hungerSpeedModifier;
    
    private Collider[] _overlapResults = new Collider[OVERLAP_SPHERE_ARRAY_SIZE];
    
    private readonly List<IGrabbable> _grabSlots = new List<IGrabbable>();
    
    public bool IsGrabbingObjects => _grabSlots.Count > 0;
    public bool HasBodyGuard;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _initialMass = _playerRigidbody.mass;
        _playerContext = GetComponent<PlayerContext>();
    }

    void Update()
    {
        _overlapResults = new Collider[OVERLAP_SPHERE_ARRAY_SIZE];
        GetObjectsInRange(_overlapResults, _interactionRadius, _interactLayer);
        HandlePickup();
        HandleUse();
        HandleHarvest();
        HandleDismantle();
    }
    
    
    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, _grabRadius);
    }

    private void HandleDismantle()
    {
        var buttonOneMinusDelay = _playerContext.InputController.InputData.Button1DownDuration - _dismantleDelay;
        if (buttonOneMinusDelay < 0f)
        {
            return;
        }

        TryDismantlePlaceablesInRange();
    }

    private void TryDismantlePlaceablesInRange()
    {
        foreach (var targetObject in _overlapResults)
        {
            if (targetObject == null) continue;
            var targetPlaceable = targetObject.GetComponent<IPlaceable>();
            if(targetPlaceable == null) continue;
            if (targetPlaceable.IsGrabbed) continue;
            TickDismantlePlaceable(targetPlaceable);
            break;
        }
    }

    private void TickDismantlePlaceable(IPlaceable placeableToDismantle)
    {
        placeableToDismantle.UnpackedPercent = placeableToDismantle.UnpackedPercent -
                                               Time.deltaTime / placeableToDismantle.UnpackDuration;
        if (placeableToDismantle.UnpackedPercent <= 0f)
        {
            placeableToDismantle.UnpackedPercent = 0f;
            placeableToDismantle.Dismantle(transform);
            placeableToDismantle.IsGrabbed = true;
            var grabbable = placeableToDismantle as IGrabbable;
            GrabGrabbable(grabbable);
            UpdatePlayerMass();
        }
    }

    private void HandleHarvest()
    {
        if (_playerContext.InputController.InputData.Button2DownDuration <= 0f)
        {
            return;
        }
        TryHarvestInRange();
    }
    
    private void HandleUse()
    {
        if (!_playerContext.InputController.InputData.Button2Down)
        {
            return;
        }

        TryConsumeItem();
    }

    public bool TryGiveItem(ItemType itemType)
    {
        for (var slotIndex = 0; slotIndex < _grabSlots.Count; slotIndex++)
        {
            var grabSlot = _grabSlots[slotIndex];
            if (!(grabSlot is IDepositable depositable) || 
                depositable.Type != itemType)
            {
                continue;
            }
            
            ReleaseGrabbedObject(grabSlot);
            Destroy(grabSlot.Transform.gameObject);
            return true;
        }

        return false;
    }
    
    private void TryHarvestInRange()
    {
        if (IsGrabbingObjects)
        {
            return;
        }

        foreach (var objectInRange in _overlapResults)
        {
            if (objectInRange == null) continue;
            var harvestableInRange = objectInRange.GetComponent<IHarvestable>();
            if (harvestableInRange == null) continue;
            var harvestPointsDelta = Mathf.CeilToInt(_harvestRate * Time.deltaTime);
            harvestableInRange.HarvestTick(harvestPointsDelta);
            return;
        }

    }
    
    private void TryConsumeItem()
    {
        if (_grabSlots.Count == 0)
        {
            return;
        }

        var foodItem = _grabSlots[0] as FoodItem;
        if (foodItem == null)
        {
            return;
        }

        if (foodItem.IsCooked)
        {
            _playerContext.ResourceController.AddCalories(foodItem.CookedCalories);
        }
        else
        {
            _playerContext.ResourceController.AddCalories(foodItem.RawCalories);
        }
        ReleaseGrabbedObject(_grabSlots[0]);
        Destroy(foodItem.gameObject);
    }
    
    private void HandlePickup()
    {
        if (!_playerContext.InputController.InputData.Button1Down)
        {
            return;
        }

        var releasedObjects = TryReleaseObjects();

        if (!releasedObjects)
        {
            TryGrabObjects(_overlapResults);
        }
        
        UpdatePlayerMass();
    }

    private bool TryReleaseObjects()
    {
        var isGrabbingObjects = _grabSlots.Count >= _grabSlotCount;
        if (isGrabbingObjects)
        {
            TryReleaseObjectsIntoBuilding(_overlapResults);
            TryPlacePlaceable();
            ReleaseAllGrabbedObjects();
            return true;
        }

        return false;
    }

    private void TryPlacePlaceable()
    {
        for (var grabSlotIndex = 0; grabSlotIndex < _grabSlots.Count; grabSlotIndex++)
        {
            var grabbable = _grabSlots[grabSlotIndex];
            var placeable = grabbable as IPlaceable;
            if (placeable == null) continue;

            ReleaseGrabbedObject(grabbable, false);
            placeable.Place();
            placeable.IsGrabbed = false;
            UpdatePlayerMass();
        }
    }

    private void TryReleaseObjectsIntoBuilding(Collider[] objectsInRange)
    {
        foreach (var objectInRange in objectsInRange)
        {
            if (objectInRange == null) continue;
            var containerInRange = objectInRange.GetComponent<IContainer>();
            if(containerInRange == null) continue;
            for (var grabSlotIndex = 0; grabSlotIndex < _grabSlots.Count; grabSlotIndex++)
            {
                var grabbable = _grabSlots[grabSlotIndex];
                var depositable = grabbable as IDepositable;
                if (depositable == null)
                {
                    continue;
                }
                var containerAsDepositable = containerInRange as IDepositable;
                if (depositable == containerAsDepositable)
                {
                    continue;
                }
                if (!containerInRange.HasEmptySlots)
                {
                    continue;
                }
                
                ReleaseGrabbedObject(grabbable);
                var depositedItem = containerInRange.TryDepositItem(depositable);
                if (depositedItem &&
                    containerInRange is IConsumer consumer)    
                {
                    consumer.Pay(depositable.Type);
                }
            }
        }
    }
    
    private void TryGrabObjects(Collider[] objects)
    {
        foreach (var targetObject in objects)
        {
            if (targetObject == null) break;
            if (TryWithdrawFromContainer(targetObject))
            {
                break;
            }
            if (TryGrabObject(targetObject))
            {
                break;
            }
        }
    }

    private void GetObjectsInRange(Collider[] objectsInRange, float radius, LayerMask layerMask)
    {
        var overlapPosition = transform.position + Vector3.up * radius;
        Physics.OverlapSphereNonAlloc(overlapPosition, radius, objectsInRange, layerMask);
    }

    private bool TryWithdrawFromContainer(Collider objectCollider)
    {
        var container = objectCollider.GetComponent<IContainer>();
        if (container == null)
        {
            return false;
        }
        
        var isWithdrawn = container.TryWithdrawFirstItem(out var withdrawnItem);
        if (!isWithdrawn)
        {
            return false;
        }
        var withdrawnGrabbable = withdrawnItem as IGrabbable;
        TryGrabGrabbable(withdrawnGrabbable);

        return true;
    }
    
    private bool TryGrabObject(Collider objectCollider)
    {
        var grabbable = objectCollider.GetComponent<IGrabbable>();
        if (grabbable == null)
        {
            return false;
        }
        
        return TryGrabGrabbable(grabbable);
    }

    private bool TryGrabGrabbable(IGrabbable grabbable)
    {
        var vectorToGrabbable = grabbable.Transform.position - transform.position;
        vectorToGrabbable.y = 0f;
        var groundAngleToHitObject = Vector3.Angle(vectorToGrabbable, transform.forward);

        if (!(groundAngleToHitObject <= _grabConeAngle))
        {
            return false;
        }

        if (grabbable is IPlaceable placeable &&
            !placeable.IsPacked)
        {
            return false;
        }
        
        if (grabbable is IDepositable depositable &&
            depositable.IsDeposited)
        {
            if (!depositable.Container.TryWithdrawItem(depositable))
            {
                return false;
            }
        }
        
        GrabGrabbable(grabbable);
        return true;
    }

    private void UpdatePlayerMass()
    {
        var grabbedMass = 0f;
        for (var grabSlotIndex = 0; grabSlotIndex < _grabSlots.Count; grabSlotIndex++)
        {
            if (_grabSlots[grabSlotIndex] == null)
            {
                continue;
            }
            grabbedMass += _grabSlots[grabSlotIndex].Rigidbody.mass;
        }
        _playerRigidbody.mass = _initialMass + grabbedMass;
    }
    
    private void GrabGrabbable(IGrabbable grabbable)
    {
        _grabSlots.Add(grabbable);
        grabbable.OnGrab(_playerContext);
        grabbable.Transform.parent = transform;
        grabbable.Rigidbody.isKinematic = true;
        grabbable.GrabCoroutine = StartCoroutine(TweenUtil.EaseTransformToPoint(grabbable.Transform, _grabPivot, _grabEaseModifier, GRAB_PIVOT_SNAP_THRESHOLD));
    }

    public void ReleaseAllGrabbedObjects()
    {
        for (var grabSlotIndex = _grabSlots.Count-1; grabSlotIndex >= 0; grabSlotIndex--)
        {
            ReleaseGrabbedObject(_grabSlots[grabSlotIndex]);
        }
    }

    private void ReleaseGrabbedObject(IGrabbable grabbedObject, bool setIsKinematic = true)
    {
        if (grabbedObject.Transform.parent)
        {
            grabbedObject.Transform.parent = null;
        }
        if (setIsKinematic)
        {
            grabbedObject.Rigidbody.isKinematic = false;
        }
        if (grabbedObject.GrabCoroutine != null)
        {
            StopCoroutine(grabbedObject.GrabCoroutine);
        }
        
        grabbedObject.OnRelease();
        _grabSlots.Remove(grabbedObject);
    }
}

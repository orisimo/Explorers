using UnityEngine;

namespace Items
{
    public class Rope : BaseGrabbable
    {
        [SerializeField] private Joint _jointToAttachTo;
        [SerializeField] private VehicleMovementController _vehicleMovementController;

        private Vector3 _initialLocalPosition;

        protected override void Awake()
        {
            base.Awake();
            _initialLocalPosition = transform.localPosition;
        }

        public override void OnGrab(PlayerContext grabbingPlayerContext)
        {
            base.OnGrab(grabbingPlayerContext);
            
            _jointToAttachTo.connectedBody = grabbingPlayerContext.Rigidbody;
            _vehicleMovementController.SetBreaks(false);
        }

        public override void OnRelease()
        {
            base.OnRelease();
            transform.SetParent(_jointToAttachTo.transform);
            transform.localPosition = _initialLocalPosition;
            _jointToAttachTo.connectedBody = Rigidbody;
            _vehicleMovementController.SetBreaks(true);
        }
        
    }
}

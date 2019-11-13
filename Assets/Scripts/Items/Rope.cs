using UnityEngine;

namespace Items
{
    public class Rope : BaseGrabbable
    {
        [SerializeField] private Joint _jointToAttachTo;

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
        }

        public override void OnRelease()
        {
            base.OnRelease();
            transform.SetParent(_jointToAttachTo.transform);
            transform.localPosition = _initialLocalPosition;
            _jointToAttachTo.connectedBody = Rigidbody;
        }
        
    }
}

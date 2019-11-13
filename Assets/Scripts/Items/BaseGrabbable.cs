using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Transform))]
    [RequireComponent(typeof(Collider))]
    public class BaseGrabbable : MonoBehaviour, IGrabbable
    {
        private Rigidbody _rigidbody;
        private Transform _transform;
        private Collider _collider;
        
        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _transform = GetComponent<Transform>();
            _collider = GetComponent<Collider>();
        }
        
        public virtual void OnGrab(PlayerContext grabbingPlayerContext)
        {
            _collider.enabled = false;
        }

        public virtual void OnRelease()
        {
            _collider.enabled = true;
        }
        
        public Rigidbody Rigidbody => _rigidbody;
        public Transform Transform => _transform;
        public Collider Collider => _collider;
        public Coroutine GrabCoroutine { get; set; }
    }
}

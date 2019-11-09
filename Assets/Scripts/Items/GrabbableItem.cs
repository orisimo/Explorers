using UnityEngine;

namespace Items
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Transform))]
    public class GrabbableItem : MonoBehaviour, IGrabbable
    {
        private Rigidbody _rigidbody;
        private Transform _transform;

        protected virtual void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _transform = GetComponent<Transform>();
        }
        
        
        public Rigidbody Rigidbody => _rigidbody;
        public Transform Transform => _transform;
        public Coroutine GrabCoroutine { get; set; }
    }
}

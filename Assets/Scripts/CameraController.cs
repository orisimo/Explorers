using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _followTarget;
    
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _fieldOfView;
    
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _lookAtTransform;

    private void Update()
    {
        _camera.transform.localPosition = _offset;
        _camera.fieldOfView = _fieldOfView;
        if (_followTarget != null)
        {
            _lookAtTransform.position = _followTarget.position;
        }
    }

}

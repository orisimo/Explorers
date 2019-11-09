using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class LookAtController : MonoBehaviour
{
    [SerializeField] private Transform _lookatTransform;

    private void Update()
    {
        if (_lookatTransform == null) return;
        transform.LookAt(_lookatTransform, _lookatTransform.up);
    }
}

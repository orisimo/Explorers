using System;
using UnityEngine;

namespace Items
{
    public interface IGrabbable
    {
        Rigidbody Rigidbody { get; }
        Transform Transform { get; }
        Coroutine GrabCoroutine { get; set; }
    }
}

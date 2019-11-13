using System;
using UnityEngine;

namespace Items
{
    public interface IGrabbable
    {
        Rigidbody Rigidbody { get; }
        Collider Collider { get; }
        Transform Transform { get; }
        Coroutine GrabCoroutine { get; set; }
        void OnGrab(PlayerContext grabbingPlayerContext);
        void OnRelease();
    }
}

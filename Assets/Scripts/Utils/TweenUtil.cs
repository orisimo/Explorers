using System.Collections;
using UnityEngine;

namespace Utils
{
    public static class TweenUtil
    {
        public static IEnumerator EaseTransformToPoint(Transform transform, Transform destinationTransform, float easeModifier = 1f, float threshold = 0.01f)
        {
            while (Vector3.Distance(transform.position, destinationTransform.position) > threshold)
            {
                transform.position = Vector3.Lerp(transform.position, destinationTransform.position, Time.deltaTime * easeModifier);
                yield return null;
            }
        }

    }
}

using UnityEngine;

namespace Utils
{
    public static class MathUtil 
    {
        public static Vector3 GetPointAroundHorizontalRadius(Vector3 targetPosition, float radius)
        {
            if (radius <= 0f)
            {
                return targetPosition;
            }
            var positionAroundHorizontalRadius = Random.insideUnitSphere * radius;
            positionAroundHorizontalRadius.y = 0f;
            positionAroundHorizontalRadius += targetPosition;
            return positionAroundHorizontalRadius;
        }

    }
}

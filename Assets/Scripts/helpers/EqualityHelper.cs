using UnityEngine;

namespace helpers
{
    public static class EqualityHelper
    {
        public static bool Rotation(Quaternion r1, Quaternion r2)
        {
            return Mathf.Abs(Quaternion.Dot(r1, r2)) >= 0.999999f;
        }
    }
}
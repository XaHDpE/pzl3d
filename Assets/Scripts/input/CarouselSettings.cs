using UnityEngine;

namespace input
{
    public static class CarouselSettings
    {
        public static float Diameter = 360.0f;
        public static Vector3 DownScale = new Vector3(0.5f, 0.5f, 0.5f);
        public static Vector3 UpScale = new Vector3(1.5f, 1.5f, 1.5f);
        public static bool resetCenterRotation = true;
        public static bool assumeObject = true;
        public static float speedOfRotation = 0.1f;
        public static float Radius = 32.0f;
    }
}
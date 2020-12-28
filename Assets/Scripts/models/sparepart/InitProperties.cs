using UnityEngine;

namespace models.sparepart
{
    public readonly struct InitProperties {
        public InitProperties(Vector3 pos, Quaternion rot)
        {
            Pos = pos;
            Rot = rot;
        }
        public Vector3 Pos { get; }

        public Quaternion Rot { get; }

        public override string ToString()
        {
            return $"InitProperties[RelativePosition: {Pos}, RelativeRotation: {Rot}]";
        }
    }
}
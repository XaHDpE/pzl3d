using System;
using UnityEngine;

namespace models.mesh
{
    public class EdgeVector : IEquatable<EdgeVector>
    {
        private readonly Vector3 _coord1;
        private readonly Vector3 _coord2;

        public EdgeVector(Vector3 coord1, Vector3 coord2)
        {
            _coord1 = coord1;
            _coord2 = coord2;
        }

        public Vector3 Coord1 => _coord1;

        public Vector3 Coord2 => _coord2;

        private static bool Eq(Vector3 lhs, Vector3 rhs)
        {
            return Vector3.SqrMagnitude(lhs - rhs) < 9.99999944E-11f;
        }

        public bool Equals(EdgeVector other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Eq(_coord1, other._coord2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(EdgeVector)) return false;
            return Equals((EdgeVector) obj);
        }

        public override int GetHashCode()
        {
            return _coord1.GetHashCode()^_coord2.GetHashCode();
        }

        public static bool operator ==(EdgeVector left, EdgeVector right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(EdgeVector left, EdgeVector right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return $"[Coord1: {_coord1}, Coord2: {_coord2} ]";
        }
    }
}
using System;
using UnityEngine;

namespace models
{
    public class Edge : IEquatable<Edge>
    {

        private readonly int _ix1;
        private readonly int _ix2;


        public Edge(int ix1, int ix2)
        {
            _ix1 = ix1;
            _ix2 = ix2;
        }

        public int Ix1 => _ix1;

        public int Ix2 => _ix2;
        public override string ToString()
        {
            return $"pair[Ix1 = {Ix1}, Ix2 = {Ix2}]";
        }


        public bool Equals(Edge other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _ix1 == other._ix2;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Edge)) return false;
            return Equals((Edge) obj);
        }

        public override int GetHashCode()
        {
            return (_ix1.GetHashCode() ^ _ix2.GetHashCode()) * 31;
        }

        public static bool operator ==(Edge left, Edge right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Edge left, Edge right)
        {
            return !Equals(left, right);
        }
    }
}
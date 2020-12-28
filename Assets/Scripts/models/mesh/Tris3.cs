using System;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace models.mesh
{
    [Serializable]
    public class Tris3
    {
        public Vector3 V0 => _transform.GetComponent<MeshFilter>().mesh.vertices[_vert0Index];
        public Vector3 V1 => _transform.GetComponent<MeshFilter>().mesh.vertices[_vert1Index];
        public Vector3 V2 => _transform.GetComponent<MeshFilter>().mesh.vertices[_vert2Index];
        
        public Vector3 V0Global => _transform.TransformPoint(V0);
        public Vector3 V1Global => _transform.TransformPoint(V1);
        public Vector3 V2Global => _transform.TransformPoint(V2);

        public Vector3 NormalGlobal => _transform.TransformDirection(Normal);

        public Vector3 MiddlePointGlobal => _transform.TransformPoint(MiddlePoint);

         public int TrisIndex { get; }

        public Vector3 Normal => Vector3.Cross(V1 - V0, V2 - V0).normalized;

        public Vector3 MiddlePoint => (V0 + V1 + V2) / 3;

        private Transform _transform;

        private int _vert0Index, _vert1Index, _vert2Index;

        public Tris3(Transform transform, int[] triangles, int trisFirstIndex)
        {
            TrisIndex = trisFirstIndex;
            _transform = transform;
            _vert0Index = triangles[trisFirstIndex * 3];
            _vert1Index = triangles[trisFirstIndex * 3 + 1];
            _vert2Index = triangles[trisFirstIndex * 3 + 2];
        }
        
        public override string ToString()
        {
            return $"Tris3: [_vert0Index: {_vert0Index}, _vert1Index: {_vert1Index}, " +
                   $"_vert2Index: {_vert2Index}, V0: {V0}, V1: {V1}, V2: {V2}]";
        }

        public void Debug(string debugText, Color lineColor, float debugDuration)
        {
            if (GameObject.Find($"Index_{TrisIndex}") != null) return;
            
            UnityEngine.Debug.DrawLine(V0Global, V1Global, lineColor, debugDuration);
            UnityEngine.Debug.DrawLine(V1Global, V2Global, lineColor, debugDuration);
            UnityEngine.Debug.DrawLine(V2Global, V0Global, lineColor, debugDuration);
            UnityEngine.Debug.DrawRay(MiddlePointGlobal, NormalGlobal * 15, lineColor, debugDuration);
            var textGameObj = new GameObject($"Index_{TrisIndex}");
            textGameObj.transform.SetParent(_transform);
            var myText = textGameObj.AddComponent<TextMeshPro>();
            myText.text = debugText;
            myText.fontSize = 4;
            myText.autoSizeTextContainer = true;
            var tmpPos = MiddlePoint + Normal * 1.1f;
            textGameObj.transform.localPosition = tmpPos;
            Object.Destroy(textGameObj, debugDuration);
        }

        public override bool Equals(object obj)
        {
            return obj is Tris3 other && TrisIndex.Equals(other.TrisIndex);
        }

        protected bool Equals(Tris3 other)
        {
            return TrisIndex == other.TrisIndex;
        }

        public override int GetHashCode()
        {
            return TrisIndex;
        }
    }
}
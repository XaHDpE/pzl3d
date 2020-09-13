using System.Collections.Generic;
using System.Linq;
using models;
using UnityEngine;

namespace helpers
{
    public static class MaterialHelper
    {
        
        public static int[] GetSubMeshIndex(MeshRenderer mr, Material m)
        {
            var mats = mr.materials;
            var inc = 0;
            var res = new int[]{};
            for (var index = 0; index < mats.Length; index++)
            {
                if (!mats[index].name.StartsWith(m.name)) continue;
                res[inc++] = index;
            }
            return res;
        }

        public static bool CheckMaterial(MeshRenderer mr, int subMeshIndex, Material materialToCheck)
        {
            return mr.materials[subMeshIndex].name.StartsWith(materialToCheck.name);
        }

        public static List<EdgeVector> CalculateBounds(Mesh mesh, int submeshIndex)
        {
            IDictionary<EdgeVector, int> dict = new Dictionary<EdgeVector, int>();

            var subMeshTri = mesh.GetTriangles(submeshIndex);
            for (var j = 0; j < subMeshTri.Length; j += 3)
            {
                var currTris = new[] {subMeshTri[j], subMeshTri[j + 1], subMeshTri[j + 2]};
                for (var k = 0; k < currTris.Length; k++)
                {
                    var ix1 = k;
                    var ix2 = (k + 1) % 3;
                    var curEdge = new EdgeVector(
                        mesh.vertices[currTris[ix1]],
                        mesh.vertices[currTris[ix2]],
                        currTris[ix1],
                        currTris[ix2]
                    );
                    if (dict.ContainsKey(curEdge)) dict[curEdge] += 1;
                    else dict.Add(curEdge, 1);
                }
            }

            return (from pair in dict where pair.Value == 1 select pair.Key).ToList();
        }


        public static int GetSubMeshIndex(Mesh m, int triangleIndex)
        {
            var hitTriangle = new[]
            {
                m.triangles[triangleIndex * 3],
                m.triangles[triangleIndex * 3 + 1],
                m.triangles[triangleIndex * 3 + 2]
            };
            for (var i = 0; i < m.subMeshCount; i++)
            {
                var subMeshTris = m.GetTriangles(i);
                for (var j = 0; j < subMeshTris.Length; j += 3)
                {
                    if (subMeshTris[j] == hitTriangle[0] &&
                        subMeshTris[j + 1] == hitTriangle[1] &&
                        subMeshTris[j + 2] == hitTriangle[2])
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
    }
}
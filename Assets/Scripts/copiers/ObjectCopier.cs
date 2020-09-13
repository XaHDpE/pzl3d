using models.sp;
using UnityEngine;

namespace copiers
{
        [CreateAssetMenu]
        public class ObjectCopier : ScriptableObject
        {

                public GameObject Copy(GameObject prefab, Transform copyFrom)
                {
                        var result = new GameObject();
                        return result;
                }

                public void ReplaceMesh(Transform target, Mesh mesh)
                {
                        var meshRendererFrom = target.GetComponent<Renderer>();
                        target.GetComponent<MeshFilter>().sharedMesh = mesh;
                }
        
                private static GameObject CopyObject(GameObject prefab, Transform copyFrom)
                {
                        var position = copyFrom.position;
                        var rotation = copyFrom.rotation;
                
                        var fbxMeshRenderer = copyFrom.GetComponent<Renderer>();
                        var result = Instantiate(prefab, position, rotation);
                        result.name = copyFrom.name;

                        var fbxMeshFilter = copyFrom.GetComponent<MeshFilter>();
                        var meshFilterGo = result.AddComponent<MeshFilter>();
                        meshFilterGo.sharedMesh = Instantiate(fbxMeshFilter.sharedMesh);
                
                        var spMeshRenderer = result.GetComponent<MeshRenderer>();
                        spMeshRenderer.sharedMaterials = fbxMeshRenderer.sharedMaterials;
                        result.AddComponent<MeshCollider>().convex = true;
                
                        return result;
                }
        
                public GameObject InstantiateSparePart(GameObject sparePartPrefab, Transform copyFrom, Transform parent)
                {
                        // var sparePart = Instantiate(sparePartPrefab, Vector3.zero, Quaternion.identity);
                        var result = CopyObject(sparePartPrefab, copyFrom);
                        result.transform.SetParent(parent);
                        return result;
                }
                
                public static SpNew CreateSparePart(GameObject sparePartPrefab, Transform copyFrom, Transform parent)
                {
                        // var sparePart = Instantiate(sparePartPrefab, Vector3.zero, Quaternion.identity);
                        var result = CopyObject(sparePartPrefab, copyFrom);
                        
                        result.transform.SetParent(parent);
                        result.AddComponent<SpNew>();
                        return result.GetComponent<SpNew>();
                }

                public static SpNew CopySpNew(Transform copyFrom, Transform parent)
                {
                        var position = copyFrom.position;
                        var rotation = copyFrom.rotation;
                        var sp = Instantiate(copyFrom, position, rotation, parent);
                        var spNew = sp.gameObject.AddComponent<SpNew>();
                        spNew.gameObject.AddComponent<MeshCollider>();
                        spNew.initProperties = new InitProperties(position, rotation);
                        return spNew;
                }

        }
}
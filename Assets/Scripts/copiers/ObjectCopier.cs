using models;
using models.sparepart;
using UnityEngine;

namespace copiers
{
        [CreateAssetMenu]
        public class ObjectCopier : ScriptableObject
        {

                public static void ReplaceMesh(MeshFilter sourceFilter, MeshFilter targetFilter)
                {
                        if (targetFilter.TryGetComponent<MeshFilter>(out var meshFilter))
                        {
                                meshFilter.sharedMesh = Instantiate(sourceFilter.sharedMesh);                                
                        }
                }

                private static void ReplaceMaterials(Renderer sourceRenderer, Renderer targetRenderer)
                {
                        targetRenderer.sharedMaterials = sourceRenderer.sharedMaterials;
                        targetRenderer.materials = sourceRenderer.materials;
                        targetRenderer.material = sourceRenderer.material;
                        targetRenderer.sharedMaterial = sourceRenderer.sharedMaterial;
                }

                public static void ReplaceMeshWithMaterials(GameObject source, GameObject target)
                {
                        if (source.TryGetComponent<MeshFilter>(out var sourceFilter) &&
                                target.TryGetComponent<MeshFilter>(out var targetFilter))
                        {
                                Debug.Log("Replace mesh called");
                                targetFilter.sharedMesh = Instantiate(sourceFilter.sharedMesh);
                                
                                if (source.TryGetComponent<Renderer>(out var sourceRenderer) &&
                                    target.TryGetComponent<Renderer>(out var targetRenderer))
                                {
                                        Debug.Log("ReplaceMaterials called");
                                        ReplaceMaterials(sourceRenderer, targetRenderer);
                                }
                                else
                                {
                                        Debug.LogError("unable to find meshRenderer components");
                                }
                        }
                        else
                        {
                                Debug.LogError("unable to find MeshFilter components");
                        }
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
                
                public static ISparePart CreateSparePart(GameObject sparePartPrefab, Transform copyFrom, Transform parent)
                {
                        // var sparePart = Instantiate(sparePartPrefab, Vector3.zero, Quaternion.identity);
                        var result = CopyObject(sparePartPrefab, copyFrom);
                        
                        result.transform.SetParent(parent);
                        result.AddComponent<SpNb0511>();
                        return result.GetComponent<SpNb0511>();
                }

                public static SpNb0511 Copy(Transform copyFrom, Transform parent)
                {
                        var position = copyFrom.position;
                        var rotation = copyFrom.rotation;
                        var sp = Instantiate(copyFrom, position, rotation, parent);
                        var spNew = sp.gameObject.AddComponent<SpNb0511>();
                        spNew.gameObject.AddComponent<MeshCollider>();
                        // spNew.initProperties = new InitProperties(position, rotation);
                        return spNew;
                }

        }
}
using UnityEngine;

namespace settings
{
    [CreateAssetMenu]
    public class MaterialData : ScriptableObject
    {
        [SerializeField] private Material selectedMaterial;

        public Material OnSelectedMaterial => selectedMaterial;
    }
}
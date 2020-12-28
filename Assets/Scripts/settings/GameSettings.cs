using UnityEngine;

namespace settings
{
    [CreateAssetMenu]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private GameObject sparePartPrefab;
        [SerializeField] private GameObject sparePartSelectedPrefab;
        [SerializeField] private GameObject spSelectedTop;

        public GameObject OnSpSelectedTop
        {
            get => spSelectedTop;
            set => spSelectedTop = value;
        }

        public GameObject SparePartPrefab
        {
            get => sparePartPrefab;
            set => sparePartPrefab = value;
        }

        public GameObject SparePartSelectedPrefab
        {
            get => sparePartSelectedPrefab;
            set => sparePartSelectedPrefab = value;
        }

    }
}

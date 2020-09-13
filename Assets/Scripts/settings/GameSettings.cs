using System;
using UnityEngine;

namespace settings
{
    [CreateAssetMenu]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private GameObject sparePartPrefab;
        [SerializeField] private GameManager gameManager;

        public GameObject SparePartPrefab
        {
            get => sparePartPrefab;
            set => sparePartPrefab = value;
        }

        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }
}

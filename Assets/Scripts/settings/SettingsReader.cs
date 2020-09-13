using UnityEditor;
using UnityEngine;

namespace settings
{
    public class SettingsReader : MonoBehaviour
    {
        public static SettingsReader Instance { get; private set; }

        [SerializeField] private GameSettings gameSettings;
        public GameSettings GameSettings => gameSettings;

        private void Awake()
        {
            Instance = this;
        }
    }
}
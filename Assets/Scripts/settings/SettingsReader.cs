using UnityEngine;

namespace settings
{
    public class SettingsReader : MonoBehaviour
    {
        private static SettingsReader instance;

        public static SettingsReader Instance => instance;

        private void Awake()
        {
            instance = this;
        }

        [SerializeField] private GameSettings gameSettings;

        public GameSettings GameSettings => gameSettings;
    }
}
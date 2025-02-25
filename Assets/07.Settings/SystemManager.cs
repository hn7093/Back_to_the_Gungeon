using UnityEngine;

namespace Preference
{

    public class SystemManager : MonoBehaviour
    {
        private static SystemManager _instance;
        public static SystemManager Instance => _instance;

        public FileManager FileManager;
        public AudioManager AudioManager;
        public UIManager UIManager;

        private void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
using UnityEngine;

namespace Preference
{

    // fix: 직접 등록에서 주입 방식으로 변경
    public class SystemManager : MonoBehaviour
    {
        private static SystemManager _instance;
        public static SystemManager Instance => _instance;

        [HideInInspector] public FileManager FileManager;
        [HideInInspector] public AudioManager AudioManager;
        [HideInInspector] public UIManager UIManager;
        [HideInInspector] public EventManager EventManager;
        [HideInInspector] public PlayerManager PlayerManager;

        private void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
            
            FileManager = GetComponentInChildren<FileManager>();
            AudioManager = GetComponentInChildren<AudioManager>();
            UIManager = GetComponentInChildren<UIManager>();
            EventManager = GetComponentInChildren<EventManager>();
        }

        public void RegisterPlayer(GameObject player)
        {
            if (PlayerManager == null) PlayerManager = new PlayerManager();
            if (!player.GetComponent<StatHandler>() || !player.GetComponent<ResourceController>()) {
                Debug.LogError("game object is missing stat handler and resource controller");
                return;
            }
            PlayerManager.RegistryInfo(player);

        }
    }
}
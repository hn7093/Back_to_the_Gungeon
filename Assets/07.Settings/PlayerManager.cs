using UnityEditor.PackageManager;
using UnityEngine;

namespace Preference
{
    public class PlayerManager
    {
        private StatHandler _statHandler;
        private ResourceController _resourceController;

        public void RegistryInfo(GameObject player)
        {
            _statHandler = player.GetComponent<StatHandler>();
            _resourceController = player.GetComponent<ResourceController>();
        }
    }
}
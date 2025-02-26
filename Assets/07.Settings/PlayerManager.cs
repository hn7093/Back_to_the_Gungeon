using UnityEditor.PackageManager;
using UnityEngine;

namespace Preference
{
    public class PlayerManager
    {
        public StatHandler StatHandler;
        public ResourceController ResourceController;

        public void RegistryInfo(GameObject player)
        {
            StatHandler = player.GetComponent<StatHandler>();
            ResourceController = player.GetComponent<ResourceController>();
        }
    }
}
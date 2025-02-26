using UnityEditor.PackageManager;
using UnityEngine;

namespace Preference
{
    
    public class PlayerStatusManager
    {
        public StatHandler StatHandler;
        public ResourceController ResourceController;

        public void RegistryInfo(GameObject player)
        {
            StatHandler = player.GetComponent<StatHandler>();
            ResourceController = player.GetComponent<ResourceController>();
        }

        public void IncreaseAbility(string methodName)
        {
            switch (methodName)
            {
                case "increase_max_health": 
                    break;
                case "increase_current_health": 
                    break;
                case "increase_attack_power": 
                    break;
                case "increase_attack_speed": 
                    break;
                case "increase_move_speed": 
                    break;
                case "Double": 
                    break;
                case "Penetration": 
                    break;
                case "Reflection": 
                    break;
                case "Multi": 
                    break;
            }
        }
    }
}
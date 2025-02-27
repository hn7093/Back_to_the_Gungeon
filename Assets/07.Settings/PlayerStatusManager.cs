using UnityEditor.PackageManager;
using UnityEngine;

namespace Preference
{
    
    public class PlayerStatusManager
    {
        private PlayerController playerController;
        [HideInInspector] public AbilityManager AbilityManager;

        public void RegistryInfo(GameObject player)
        {
            playerController = player.GetComponent<PlayerController>();
            if (AbilityManager == null) AbilityManager = new AbilityManager();
        }

        public void IncreaseAbility(string methodName)
        {
            
            switch (methodName)
            {
                case "increase_max_health":
                    playerController.AddMaxHP(100);
                    break;
                case "increase_current_health": 
                    playerController.ChangeHealth(100);
                    break;
                case "increase_attack_power": 
                    playerController.AddPower(10);
                    break;
                case "increase_attack_speed": 
                    playerController.AddAttackSpeed(10);
                    break;
                case "increase_move_speed": 
                    playerController.AddSpeed(10);
                    break;
                case "Double": 
                    playerController.AddBullet(1);
                    break;
                case "Penetration": 
                    playerController.SetBounce(true);
                    break;
                case "Reflection": 
                    playerController.SetThrough(true);
                    break;
                case "Multi": 
                    break;
            }
        }

        public void DecreaseCurrentHealth()
        {
            playerController.ChangeHealth(-50);
        }

        // public string GetInfoText()
        // {
        //     StatHandler StatHandler = playerController.GetComponent<StatHandler>();
        //     WeaponHandler WeaponHandler = playerController.GetComponent<WeaponHandler>();
        //
        //     return $"스테이지: {SystemManager.Instance.FileManager.CurrentStage}\n"
        //         + $"체력: {StatHandler.Health}\n";
        //         + $"공격력: {WeaponHandler.Power.ToString()}";}
        // }
    }
}
using UnityEngine;


namespace Preference
{
    public class UIManager : MonoBehaviour
    {

        public enum Pages { START_PAGE, SETTINGS_PAGE, GAMEOVER_PAGE }
        
        private static UIManager _instance;
        public static UIManager Instance => _instance;
        
    }
}


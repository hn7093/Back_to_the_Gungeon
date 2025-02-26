using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Preference
{
    public enum PageType
    {
        HOME_PAGE,
        MAIN_PAGE,
        SETTINGS_PAGE,
        GAMEOVER_PAGE,
        ANGEL_PAGE
    }
    
    [System.Serializable]
    public class PageObject
    {
        public PageType PageName;
        public GameObject Instance;
    }
    
    public abstract class UIMonoBehaviour : MonoBehaviour
    {
        protected SystemManager systemManager;
        protected FileManager fileManager;
        protected AudioManager audioManager;
        protected UIManager uiManager;
        protected EventManager eventManager;
        protected KeyBinding keyBinding;
        
        public void connectUIMnager(UIManager uiManager)
        {
            this.uiManager = uiManager;
            // fix: Awake 순서상의 문제로 등록되지 못한 경우 발생
            systemManager = SystemManager.Instance;
            fileManager = systemManager.FileManager;
            audioManager = systemManager.AudioManager;
            eventManager = systemManager.EventManager;
            keyBinding = systemManager.KeyBinding;
        }
    }
    
    // fix: 싱글톤 -> 글로벌 매니저 내부에서 관리
    public class UIManager : MonoBehaviour
    {
        
        public List<PageObject> Pages;
        private PageObject _currentPage;
        
        public bool isOpenStartPage = false;

        // fix 순서상의 이유로 Start에서 
        private void Start()
        {
            Clear();
            if (isOpenStartPage)
            {
                _currentPage = Pages[0];
                _currentPage.Instance.SetActive(true);
            }
        }

        public void Clear()
        {
            foreach (PageObject page in Pages)
            {
                GameObject pageInstance = page.Instance;
                pageInstance.SetActive(false);
                pageInstance.GetComponent<UIMonoBehaviour>()?.connectUIMnager(this);
            }
        }

        public void GoTo(PageType pageName)
        {
            _currentPage?.Instance.SetActive(false);
            // 없는 경우에 대한 알림 필요
            _currentPage = Pages.Find(page => page.PageName == pageName);
            _currentPage.Instance.SetActive(true);
        }
    }
}


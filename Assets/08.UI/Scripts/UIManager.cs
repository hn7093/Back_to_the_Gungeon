using System.Collections.Generic;
using UnityEngine;

namespace Preference
{
    public enum PageType
    {
        HOME_PAGE,
        MAIN_PAGE,
        SETTINGS_PAGE,
        GAMEOVER_PAGE
    }
    
    [System.Serializable]
    public class PageObject
    {
        public PageType PageName;
        public GameObject Instance;
    }
    
    public abstract class UIMonoBehaviour : MonoBehaviour {
        protected SystemManager systemManager = SystemManager.Instance;
        protected UIManager _uiManager;
        
        public void connectUIMnager(UIManager uiManager)
        {
            _uiManager = uiManager;
        }
    }
    
    // fix: 싱글톤 -> 글로벌 매니저 내부에서 관리
    public class UIManager : MonoBehaviour
    {
        
        public List<PageObject> Pages;
        private PageObject _currentPage;

        private void Awake()
        {
            foreach (PageObject page in Pages)
            {
                GameObject pageInstance = page.Instance;
                pageInstance.SetActive(false);
                pageInstance.GetComponent<UIMonoBehaviour>()?.connectUIMnager(this);
            }
            
            _currentPage = Pages[0];
            _currentPage.Instance.SetActive(true);
        }

        public void GoTo(PageType pageName)
        {
            _currentPage.Instance.SetActive(false);
            // 없는 경우에 대한 알림 필요
            _currentPage = Pages.Find(page => page.PageName == pageName);
            _currentPage.Instance.SetActive(true);
        }
    }
}


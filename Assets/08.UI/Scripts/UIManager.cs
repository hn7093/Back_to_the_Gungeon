using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Preference
{
    public enum PageType
    {
        HOME_PAGE,
        INTRO_PAGE,
        MAIN_PAGE,
        SETTINGS_PAGE,
        GAMEOVER_PAGE,
        ANGEL_PAGE,
        DEVIL_PAGE,
        ROULETTE_PAGE,
        STAGE_CLEAR_PAGE,
        DIALOGUE_PAGE, // 대화 데이터까지 포함하여
        IVENTORY_PAGE,
        PAUSE_PAGE,
        QUEST_PAGE,
        QUEST_COMPLETE_PAGE,
        TUTORIAL_PAGE,
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
        
        public void connectUIMnager(UIManager uiManager)
        {
            this.uiManager = uiManager;
            // fix: Awake 순서상의 문제로 등록되지 못한 경우 발생
            systemManager = SystemManager.Instance;
            fileManager = systemManager.FileManager;
            audioManager = systemManager.AudioManager;
            eventManager = systemManager.EventManager;
        }
    }
    
    // fix: 싱글톤 -> 글로벌 매니저 내부에서 관리
    public class UIManager : MonoBehaviour
    {
        
        public List<PageObject> Pages;
        private PageObject _currentPage;
        public bool isLobby = true;
        
        public bool isOpenStartPage = false;

        private void Start()
        {
            Clear();
            if (isOpenStartPage)
            {
                _currentPage = Pages[0];
                Debug.Log(_currentPage.Instance);
                _currentPage.Instance.SetActive(true);
            }
        }

        public void Clear()
        {
            foreach (PageObject page in Pages)
            {
                GameObject pageInstance = page.Instance;

                if (pageInstance)
                {
                    pageInstance?.SetActive(false);
                    pageInstance.GetComponent<UIMonoBehaviour>()?.connectUIMnager(this);
                }
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void OpenPage(PageType pageName)
        {
            // Debug.Log(pageName);
            if (_currentPage?.Instance) {
                _currentPage?.Instance?.SetActive(false);
            }
            
            // hofix: 홈으로 가는 경우로 해둔 목록들, lobby 체크를 통해 비활성화
            if (pageName == PageType.HOME_PAGE && !isLobby) { return; } 
            
            // do: 없는 경우에 대한 예외 처리
            _currentPage = Pages.Find(page => page.PageName == pageName);
            _currentPage.Instance.SetActive(true);
        }

        private PageType? _currentOpenPage;

        // ReSharper disable Unity.PerformanceAnalysis
        private void TogglePage(PageType pageName)
        {
            if (isLobby) return;
            
            if (_currentOpenPage == pageName)
            {
                Clear();
                _currentOpenPage = null;
                return;
            }
            OpenPage(pageName);
            _currentOpenPage = pageName;
        }


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                TogglePage(PageType.QUEST_PAGE);
            }
        }
    }
}


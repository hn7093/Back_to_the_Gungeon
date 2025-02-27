using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static MonsterName;

public enum QuestType { DESTORY_MONSTER, Tutorial }
public enum MonsterName { GOBLIN, FLY }

namespace Preference
{

    public class EventManager: MonoBehaviour
    {

        private TaskCompletionSource<string> _selectionTask;

        public void NotifyTaskComplete(string value)
        {
            _selectionTask?.TrySetResult(value);
        }
        
        public Task<string> GetEventResult()
        {
            _selectionTask = new TaskCompletionSource<string>();
            return _selectionTask.Task;
        }
        
        // Task
        // ReSharper disable Unity.PerformanceAnalysis
        public async Task<string> OpenEventPage(PageType pageType)
        {
            _selectionTask = null;
            UIManager uiManager = SystemManager.Instance.UIManager;
            PlayerStatusManager playerStatusManager = SystemManager.Instance.PlayerStatusManager;
            
            uiManager.OpenPage(pageType);
            string selectedValue = await GetEventResult();

            if (pageType == PageType.DEVIL_PAGE && selectedValue != null)
            {
                uiManager.Clear();
                return selectedValue;
            }
            
            playerStatusManager.IncreaseAbility(selectedValue);
            
            uiManager.Clear();
            return selectedValue;
        }


        private List<MonsterName> DestoryStackByQuest;

        public bool isQuest1Clear = false;
        public bool isQuest2Clear = false;
        
        public enum TutorialList
        {
            MOVE_ARROW,
            DESTORY_MONSTER,
            GET_REWARD,
        }
        
        public void Start()
        {
            DestoryStackByQuest = new List<MonsterName>();
        }

        public void DestroyDetector(MonsterName name)
        {
            // Debug.Log(name);
            DestoryStackByQuest.Add(name);
            CheckQuestClear();
        }

        private void CheckQuestClear()
        {
            if (!isQuest1Clear)
            {
                int goblinCount = DestoryStackByQuest.FindAll(name => name == MonsterName.GOBLIN).Count;
                if (goblinCount == 3)
                {
                    isQuest1Clear = true;
                    SystemManager.Instance.UIManager.OpenPage(PageType.QUEST_COMPLETE_PAGE);
                }
            }
           
            if (!isQuest2Clear)
            {
                int goblinCount = DestoryStackByQuest.FindAll(name => name == MonsterName.FLY).Count;
                if (goblinCount == 3)
                {
                    isQuest2Clear = true;
                    SystemManager.Instance.UIManager.OpenPage(PageType.QUEST_COMPLETE_PAGE);
                }
            }
        }
        
        
        public bool isTutorial1Clear = false;
        public bool isTutorial2Clear = false;
        public bool isTutorial3Clear = false;

        public void TutorialStart()
        {
            UIManager uiManager = SystemManager.Instance.UIManager;
            uiManager.OpenPage(PageType.TUTORIAL_PAGE);
            
        }
    }
}
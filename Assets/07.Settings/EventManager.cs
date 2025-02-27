using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public enum QuestType { DESTORY_MONSTER, Tutorial }
public enum MonsterName { GOBLINE, FLY }

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

        public void DestroyDetector(MonsterName monster)
        {
            DestoryStackByQuest.Add(monster);
            CheckQuestClear();
        }

        private void CheckQuestClear()
        {
               
        }
    }
}
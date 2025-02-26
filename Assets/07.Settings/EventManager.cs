using System.Threading.Tasks;
using UnityEngine;

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
            playerStatusManager.IncreaseAbility(selectedValue);
            
            uiManager.Clear();
            return selectedValue;
        }
    }
}
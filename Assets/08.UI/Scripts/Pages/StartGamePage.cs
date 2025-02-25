using Preference;
using UnityEngine;

public class StartGamePage : UIMonoBehaviour
{
    
    public void StartGame()
    {
        Debug.Log("StartGame");
    }

    public void LoadGame()
    {
        systemManager.FileManager.LoadGameData();
    }

    public void GotoSettings()
    {
        systemManager.UIManager.GoTo(PageType.SETTINGS_PAGE);
    }

    public void QuitGame()
    {
        systemManager.FileManager.ExitGame();
    }
    
}

using Preference;
using UnityEngine.SceneManagement;

public class GameOverPage : UIMonoBehaviour
{
    private string _startScenePath = "";
    
    public void RestartGame()
    {
        systemManager.FileManager.StartGame();
    }

    public void ExitGame()
    {
        // SceneManager.LoadScene(_startScenePath);
    }
}

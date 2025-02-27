using System.IO;
using Preference;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatus
{
    
}

public class FileManager : MonoBehaviour
{
    private string _startScene = "Tutorial";
    private string _lobbyScene = "Lobby";
    
    private string _filePath = Application.streamingAssetsPath + "gameData.json";
    // private string _playerPrefName = "GAME_DATA";
    public int CurrentStage = 0;
    
    public static readonly string controlTypeKey = "controlTypeKey";

    public void UpdateControlType(int index)
    {
        PlayerPrefs.SetInt(controlTypeKey, index);
    }

    public int GetControlType()
    {
        return PlayerPrefs.GetInt(controlTypeKey);
    }

    public void StartGame()
    {
        var uiManager = SystemManager.Instance.UIManager;
        uiManager.Clear();
        uiManager.isLobby = false;
        uiManager.isOpenStartPage = false;
        
        SceneManager.LoadScene(_startScene);
    }

    public void GoToLobby()
    {
        var uiManager = SystemManager.Instance.UIManager;
        
        uiManager.Clear();
        uiManager.isLobby = true;
        uiManager.isOpenStartPage = true;
        uiManager.OpenPage(PageType.HOME_PAGE);
        
        SceneManager.LoadScene(_lobbyScene);
    }

    public void SaveGameData(GameStatus gameStatus)
    {
        string json = JsonUtility.ToJson(gameStatus);
        File.WriteAllText(_filePath, json);
    }

    public GameStatus LoadGameData()
    {
        if(!File.Exists(_filePath)) { return new GameStatus(); }
        string json = File.ReadAllText(_filePath);
        return JsonUtility.FromJson<GameStatus>(json);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

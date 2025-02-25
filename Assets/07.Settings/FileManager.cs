using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatus
{
    
}

public class FileManager : MonoBehaviour
{
    private string _startScene = "";
    private string _filePath = Application.streamingAssetsPath + "gameData.json";
    // private string _playerPrefName = "GAME_DATA";

    public void StartGame()
    {
        SceneManager.LoadScene(_startScene);
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

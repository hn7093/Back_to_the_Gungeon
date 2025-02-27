using Preference;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverPage : UIMonoBehaviour
{
    public Button ExitButton;
    public Button RestartButton;
    public Button RetryButton;
    
    private void OnEnable() { audioManager.PlayVFXSoundByName("die"); }

    private void Start()
    {
        ExitButton.onClick.AddListener(() => SystemManager.Instance.FileManager.GoToLobby());
    }
}

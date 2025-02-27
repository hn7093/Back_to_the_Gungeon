using Preference;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class GameOverPage : UIMonoBehaviour
{
    public Button ExitButton;
    public Button RestartButton;
    public Button RetryButton;
    public TextMeshProUGUI StageText;
    
    private void OnEnable() { audioManager.PlayVFXSoundByName("die"); }

    private void Start()
    {
        StageManager stageManager = FindObjectOfType<StageManager>();
        if(stageManager != null && StageText != null)  StageText.text = "결과 : " + stageManager.stageCount.ToString() + " 스테이지";
        ExitButton.onClick.AddListener(() => SystemManager.Instance.FileManager.GoToLobby());
    }
}
